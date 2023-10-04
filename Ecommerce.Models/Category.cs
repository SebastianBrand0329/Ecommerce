using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Nombre Categoría")]
        [MaxLength(100, ErrorMessage = " Debe ser de {1} caracteres máximo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(100, ErrorMessage = "Debe ser de {1} caracteres máximo")]
        [Required(ErrorMessage ="El campo {0} es obligatorio")]
        public string Description { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public bool State { get; set; }
    }
}
