using CONTACTO_CORE.Domain.Model;

namespace CONTACTO_CORE.Repositories
{
    public interface IContactoRepository
    {
        List<Contacto> ObtenerTodos();
        Contacto? ObtenerPorId(int id);
        Contacto? AgregarSiNoExiste(string nombre, string telefono);
    }
}
