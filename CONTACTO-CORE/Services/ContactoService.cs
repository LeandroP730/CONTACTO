using CONTACTO_CORE.Domain.Model;
using CONTACTO_CORE.Domain.Response;
using CONTACTO_CORE.Dto;
using CONTACTO_CORE.Repositories;
using Microsoft.Extensions.Logging;

namespace CONTACTO_CORE.Services
{
    public class ContactoService : IContactoService
    {
        private readonly IContactoRepository _repository;

        public ContactoService(IContactoRepository repository)
        {
            _repository = repository;
        }

        public List<ContactoDto> Obtener()
        {
            var contactos = _repository.ObtenerTodos();

            return contactos.Select(Map).ToList();
        }

        public Result<ContactoDto> ObtenerPorId(int id)
        {
            var contacto = _repository.ObtenerPorId(id);

            if (contacto == null)
            {
                return Result<ContactoDto>.Failure(
                    "Contacto no encontrado",
                    ResultErrorType.NotFound
                );
            }

            return Result<ContactoDto>.Success(Map(contacto));
        }

        public Result<ContactoDto> Crear(CrearContactoRequest request)
        {
            var existe = _repository.ExisteTelefono(request.Telefono);

            if (existe)
            {
                return Result<ContactoDto>.Failure(
                    "El teléfono ya existe",
                    ResultErrorType.Conflict
                );
            }

            var contacto = _repository.Agregar(
                request.Nombre,
                request.Telefono
            );

            return Result<ContactoDto>.Success(Map(contacto));
        }

        private ContactoDto Map(Contacto contacto)
        {
            return new ContactoDto
            {
                Id = contacto.Id,
                Nombre = contacto.Nombre,
                Telefono = contacto.Telefono
            };
        }
    }
}
