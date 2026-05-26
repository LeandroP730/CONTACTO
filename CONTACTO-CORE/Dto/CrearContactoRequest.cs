using System.ComponentModel.DataAnnotations;

namespace CONTACTO_CORE.Dto
{
    public class CrearContactoRequest
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "El teléfono debe tener entre 7 y 20 caracteres.")]
        public string Telefono { get; set; } = string.Empty;
    }
}
