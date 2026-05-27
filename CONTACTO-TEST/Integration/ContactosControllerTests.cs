using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CONTACTO_TEST.Integration
{
    public class ContactosControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private const string BaseUrl = "/api/v1/contactos";

        public ContactosControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetContactos_Retorna200()
        {
            var response = await _client.GetAsync(BaseUrl);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetContacto_Inexistente_Retorna404()
        {
            var response = await _client.GetAsync($"{BaseUrl}/99999");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetContacto_Inexistente_CuerpoConTipoNotFound()
        {
            var response = await _client.GetAsync($"{BaseUrl}/99999");
            var body = await LeerErrorAsync(response);

            body.GetProperty("tipo").GetString().Should().Be("not_found");
            body.GetProperty("mensaje").GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task CrearContacto_Retorna201()
        {
            var response = await _client.PostAsJsonAsync(BaseUrl, new
            {
                nombre = "Leandro Abarca",
                telefono = "987654321"
            });

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task CrearContacto_TelefonoDuplicado_Retorna409()
        {
            await _client.PostAsJsonAsync(BaseUrl, new
            {
                nombre = "Primero",
                telefono = "111222333"
            });

            var response = await _client.PostAsJsonAsync(BaseUrl, new
            {
                nombre = "Segundo",
                telefono = "111222333"
            });

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task CrearContacto_TelefonoDuplicado_CuerpoConTipoConflict()
        {
            await _client.PostAsJsonAsync(BaseUrl, new
            {
                nombre = "Alpha",
                telefono = "444555666"
            });

            var response = await _client.PostAsJsonAsync(BaseUrl, new
            {
                nombre = "Beta",
                telefono = "444555666"
            });

            var body = await LeerErrorAsync(response);
            body.GetProperty("tipo").GetString().Should().Be("conflict");
            body.GetProperty("mensaje").GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task CrearContacto_NombreVacio_Retorna400()
        {
            var response = await _client.PostAsJsonAsync(BaseUrl, new
            {
                nombre = "",
                telefono = "555444333"
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CrearContacto_NombreVacio_CuerpoConTipoValidationError()
        {
            var response = await _client.PostAsJsonAsync(BaseUrl, new
            {
                nombre = "",
                telefono = "777888999"
            });

            var body = await LeerErrorAsync(response);
            body.GetProperty("tipo").GetString().Should().Be("validation_error");
            body.GetProperty("mensaje").GetString().Should().NotBeNullOrWhiteSpace();
            body.GetProperty("errores").GetArrayLength().Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task CrearContacto_TelefonoCorto_CuerpoConErroresDeValidacion()
        {
            var response = await _client.PostAsJsonAsync(BaseUrl, new
            {
                nombre = "Valido",
                telefono = "123"
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await LeerErrorAsync(response);
            body.GetProperty("tipo").GetString().Should().Be("validation_error");
            body.GetProperty("errores").GetArrayLength().Should().BeGreaterThan(0);
        }


        [Fact]
        public async Task CrearContacto_Concurrente_SoloUnRegistroGana()
        {
            await using var factory = new WebApplicationFactory<Program>();
            var cliente = factory.CreateClient();

            const string telefonoConcurrente = "999888777";
            const int cantidadRequests = 20;

            var tareas = Enumerable
                .Range(0, cantidadRequests)
                .Select(i => cliente.PostAsJsonAsync(BaseUrl, new
                {
                    nombre = $"Concurrente {i}",
                    telefono = telefonoConcurrente
                }))
                .ToList();

            var respuestas = await Task.WhenAll(tareas);

            var creados    = respuestas.Count(r => r.StatusCode == HttpStatusCode.Created);
            var conflictos = respuestas.Count(r => r.StatusCode == HttpStatusCode.Conflict);

            creados.Should().Be(1, "solo un hilo puede ganar la reserva atómica del teléfono");

            conflictos.Should().Be(cantidadRequests - 1);
        }

        private static async Task<JsonElement> LeerErrorAsync(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<JsonElement>(json, JsonOptions);
        }
    }
}
