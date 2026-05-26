namespace CONTACTO_CORE.Domain.Model
{
    public class Contacto
    {
        public int Id { get; init; }
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
    }
}
