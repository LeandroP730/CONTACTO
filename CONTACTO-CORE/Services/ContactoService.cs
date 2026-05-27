using CONTACTO_CORE.Domain.Model;
using CONTACTO_CORE.Domain.Response;
using CONTACTO_CORE.Dto;
using CONTACTO_CORE.Repositories;

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
            return _repository.ObtenerTodos().Select(Map).ToList();
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
            var contacto = _repository.AgregarSiNoExiste(request.Nombre, request.Telefono);

            if (contacto == null)
            {
                return Result<ContactoDto>.Failure(
                    "El teléfono ya existe",
                    ResultErrorType.Conflict
                );
            }

            return Result<ContactoDto>.Success(Map(contacto));
        }

        private static ContactoDto Map(Contacto contacto) => new()
        {
            Id = contacto.Id,
            Nombre = contacto.Nombre,
            Telefono = contacto.Telefono
        };
    }
}
