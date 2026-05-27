using CONTACTO_CORE.Domain.Model;
using System.Collections.Concurrent;

namespace CONTACTO_CORE.Repositories
{
    public class InMemoryContactoRepository : IContactoRepository
    {
        private readonly ConcurrentDictionary<int, Contacto> _contactos = new();

        private readonly ConcurrentDictionary<string, byte> _telefonos = new();

        private int _idSiguiente = 0;

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

        public Contacto? AgregarSiNoExiste(string nombre, string telefono)
        {
            if (!_telefonos.TryAdd(telefono, 0))
                return null;

            var id = Interlocked.Increment(ref _idSiguiente);

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
