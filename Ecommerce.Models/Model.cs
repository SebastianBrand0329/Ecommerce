using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Model
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nombre Modelo")]
        [MaxLength(100, ErrorMessage ="El campo {0} debe contener solo {1} caracteres")]
        [Required(ErrorMessage ="El campo {0} es obligatorio")]
        public string Name { get; set; }

        [Display(Name ="Estado")]
        [Required(ErrorMessage ="El campo {0} es obligatorio")]
        public bool State { get; set; }

    }
}
