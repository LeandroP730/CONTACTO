using CONTACTO_CORE.Domain.Response;
using CONTACTO_CORE.Dto;

namespace CONTACTO_CORE.Services
{
    public interface IContactoService
    {
        List<ContactoDto> Obtener();
        Result<ContactoDto> ObtenerPorId(int id);
        Result<ContactoDto> Crear(CrearContactoRequest request);
    }
}
