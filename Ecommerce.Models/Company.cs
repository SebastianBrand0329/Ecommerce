using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nombre Compañía")]
        [MaxLength(80)]    
        [Required(ErrorMessage ="El campo {0} es requerido")]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(200)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Description { get; set; }

        [Display(Name = "País")]
        [MaxLength(60)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Country { get; set; }

        [Display(Name = "Ciudad")]
        [MaxLength(60)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string City { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(100)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Address { get; set; }

        [Display(Name = "Telefono")]
        [MaxLength(40)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Phone { get; set; }

        [Display(Name ="Bodega de Venta")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int WarehouseId { get; set; }

        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; set; }

        public string UserCreateId { get; set; }

        [ForeignKey("UserCreateId")]
        public User UserCreate { get; set; }

        public string UserUpdateId { get; set; }

        [ForeignKey("UserUpdateId")]
        public User UserUpdate{ get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
