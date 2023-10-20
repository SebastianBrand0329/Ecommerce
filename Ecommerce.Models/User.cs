using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class User : IdentityUser
    {
        [Display(Name ="Nombre Usuario")]
        [MaxLength(80)]
        [Required(ErrorMessage ="El campo {0} es requerido")]
        public string Name { get; set; }

        [Display(Name = "Apellidos Usuario")]
        [MaxLength(80)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string LastName { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(80)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Address { get; set; }

        [Display(Name = "Ciudad")]
        [MaxLength(80)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string City { get; set; }

        [Display(Name = "País")]
        [MaxLength(80)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Country { get; set; }

        [NotMapped] // No agregate in Db
        public string Role { get; set; }
    }
}
