
using System.ComponentModel.DataAnnotations;

namespace BinaryOperationsApp.Models
{
    public class BinaryModel
    {
        [Required(ErrorMessage = "El campo 'a' es obligatorio.")]
        [RegularExpression("^[01]+$", ErrorMessage = "Solo se permiten los caracteres 0 y 1.")]
        [StringLength(8, MinimumLength = 2, ErrorMessage = "La longitud debe ser entre 2 y 8.")]
        public string? A { get; set; }

        [Required(ErrorMessage = "El campo 'b' es obligatorio.")]
        [RegularExpression("^[01]+$", ErrorMessage = "Solo se permiten los caracteres 0 y 1.")]
        [StringLength(8, MinimumLength = 2, ErrorMessage = "La longitud debe ser entre 2 y 8.")]
        public string? B { get; set; }

        public string? ResultTableHtml { get; set; }
    }
}
