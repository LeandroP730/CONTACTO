namespace CONTACTO_API.Models
{
    /// <summary>
    /// Cuerpo estándar para todas las respuestas de error de la API.
    /// </summary>
    public class ErrorResponse
    {
        public string Tipo { get; init; } = string.Empty;
        public string Mensaje { get; init; } = string.Empty;
        public IEnumerable<string>? Errores { get; init; }
    }
}
