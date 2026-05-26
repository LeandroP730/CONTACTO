using CONTACTO_CORE.Domain.Model;
using System.Collections.Concurrent;

namespace CONTACTO_CORE.Repositories
{
    public class InMemoryContactoRepository : IContactoRepository
    {
        private readonly ConcurrentDictionary<int, Contacto> _contactos = new();

        private int _id = 0;

        public List<Contacto> ObtenerTodos()
        {
            return _contactos.Values
                .OrderBy(x => x.Id)
                .ToList();
        }

        public Contacto? ObtenerPorId(int id)
        {
            _contactos.TryGetValue(id, out var contacto);

            return contacto;
        }

        public bool ExisteTelefono(string telefono)
        {
            return _contactos.Values
                .Any(x => x.Telefono == telefono);
        }

        public Contacto Agregar(string nombre, string telefono)
        {
            var id = Interlocked.Increment(ref _id);

            var contacto = new Contacto
            {
                Id = id,
                Nombre = nombre,
                Telefono = telefono
            };

            _contactos.TryAdd(id, contacto);

            return contacto;
        }
    }
}
