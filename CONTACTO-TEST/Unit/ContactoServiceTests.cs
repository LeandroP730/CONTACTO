using CONTACTO_CORE.Domain.Model;
using CONTACTO_CORE.Domain.Response;
using CONTACTO_CORE.Dto;
using CONTACTO_CORE.Repositories;
using CONTACTO_CORE.Services;
using FluentAssertions;
using Moq;

namespace CONTACTO_TEST.Unit
{
    public class ContactoServiceTests
    {
        private readonly Mock<IContactoRepository> _repoMock;
        private readonly ContactoService _service;

        public ContactoServiceTests()
        {
            _repoMock = new Mock<IContactoRepository>();
            _service = new ContactoService(_repoMock.Object);
        }

        [Fact]
        public void Obtener_SinContactos_RetornaListaVacia()
        {
            _repoMock
                .Setup(x => x.ObtenerTodos())
                .Returns(new List<Contacto>());

            var resultado = _service.Obtener();

            resultado.Should().BeEmpty();
        }

        [Fact]
        public void Obtener_ConContactos_RetornaLista()
        {
            var contactos = new List<Contacto>
            {
                new() { Id = 1, Nombre = "Leandro Pina",  Telefono = "111111111" },
                new() { Id = 2, Nombre = "Paola Pina",    Telefono = "222222222" }
            };

            _repoMock.Setup(x => x.ObtenerTodos()).Returns(contactos);

            var resultado = _service.Obtener();

            resultado.Should().HaveCount(2);
        }

        [Fact]
        public void ObtenerPorId_Existe_RetornaSuccess()
        {
            var contacto = new Contacto { Id = 1, Nombre = "Leandro Pina", Telefono = "111111111" };

            _repoMock.Setup(x => x.ObtenerPorId(1)).Returns(contacto);

            var resultado = _service.ObtenerPorId(1);

            resultado.IsSuccess.Should().BeTrue();
            resultado.Value!.Nombre.Should().Be("Leandro Pina");
        }

        [Fact]
        public void ObtenerPorId_NoExiste_RetornaNotFound()
        {
            _repoMock.Setup(x => x.ObtenerPorId(99)).Returns((Contacto?)null);

            var resultado = _service.ObtenerPorId(99);

            resultado.IsSuccess.Should().BeFalse();
            resultado.ErrorType.Should().Be(ResultErrorType.NotFound);
        }

        [Fact]
        public void Crear_TelefonoNuevo_RetornaContactoCreado()
        {
            var request = new CrearContactoRequest { Nombre = "Paola Pina", Telefono = "123456789" };
            var contacto = new Contacto { Id = 1, Nombre = "Paola Pina", Telefono = "123456789" };

            _repoMock
                .Setup(x => x.AgregarSiNoExiste("Paola Pina", "123456789"))
                .Returns(contacto);

            var resultado = _service.Crear(request);

            resultado.IsSuccess.Should().BeTrue();
            resultado.Value!.Id.Should().Be(1);
            resultado.Value.Telefono.Should().Be("123456789");
        }

        [Fact]
        public void Crear_TelefonoDuplicado_RetornaConflict()
        {
            var request = new CrearContactoRequest { Nombre = "Otro", Telefono = "123456789" };

            _repoMock
                .Setup(x => x.AgregarSiNoExiste("Otro", "123456789"))
                .Returns((Contacto?)null);

            var resultado = _service.Crear(request);

            resultado.IsSuccess.Should().BeFalse();
            resultado.ErrorType.Should().Be(ResultErrorType.Conflict);
        }

        [Fact]
        public void Crear_TelefonoDuplicado_NoLlamaAgregarNuevaVez()
        {
            // Verifica que el servicio no realiza una segunda operación de escritura.
            var request = new CrearContactoRequest { Nombre = "Duplicado", Telefono = "000000000" };

            _repoMock
                .Setup(x => x.AgregarSiNoExiste(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((Contacto?)null);

            _service.Crear(request);

            _repoMock.Verify(
                x => x.AgregarSiNoExiste(It.IsAny<string>(), It.IsAny<string>()),
                Times.Once
            );
        }
    }
}
