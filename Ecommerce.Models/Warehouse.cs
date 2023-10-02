using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Warehouse
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Nombre Bodega")]
        [MaxLength(100, ErrorMessage ="El campo {0} debe ser máximo de {1} catacteres")]    
        [Required(ErrorMessage ="El campo {0} es obligatorio")]
        public string Name { get; set; }

        [Display(Name ="Descripción")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe ser máximo de {1} catacteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Description { get; set; }

        [Display(Name ="Estado")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public bool State { get; set; }
    }
}
