using CONTACTO_CORE.Domain.Model;

namespace CONTACTO_CORE.Repositories
{
    public interface IContactoRepository
    {
        List<Contacto> ObtenerTodos();
        Contacto? ObtenerPorId(int id);
        bool ExisteTelefono(string telefono);
        Contacto Agregar(string nombre, string telefono);
    }
}
