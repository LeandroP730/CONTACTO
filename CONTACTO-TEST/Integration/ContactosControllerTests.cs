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
        private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

        public ContactosControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetContactos_Retorna200()
        {
            var response = await _client.GetAsync("/api/contactos");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CrearContacto_Retorna201()
        {
            var payload = new
            {
                nombre = "Leandro Abarca",
                telefono = "987654321"
            };

            var response = await _client.PostAsJsonAsync("/api/contactos", payload);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task CrearContacto_TelefonoDuplicado_Retorna409()
        {
            await _client.PostAsJsonAsync("/api/contactos", new
            {
                nombre = "Primero",
                telefono = "111222333"
            });

            var response = await _client.PostAsJsonAsync("/api/contactos", new
            {
                nombre = "Segundo",
                telefono = "111222333"
            });

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task CrearContacto_NombreVacio_Retorna400()
        {
            var response = await _client.PostAsJsonAsync("/api/contactos", new
            {
                nombre = "",
                telefono = "555444333"
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetContacto_Inexistente_Retorna404()
        {
            var response = await _client.GetAsync("/api/contactos/99999");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }

}
