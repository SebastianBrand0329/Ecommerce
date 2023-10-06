using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Número de Serie")]
        [MaxLength(100, ErrorMessage ="El campo {0} solo debe tener {1} caracteres")]
        [Required(ErrorMessage ="El campo {0} es obligatorio")]
        public string serialNumber { get; set; }

        [Display(Name = "Descrición del Producto")]
        [MaxLength(100, ErrorMessage = "El campo {0} solo debe tener {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Description { get; set; }

        [Display(Name = "Precio del Producto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public double Price { get; set; }

        [Display(Name = "Costo del Producto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public double Cost { get; set; }

        public string ImageUrl { get; set; }

        [Display(Name = "Estado del Producto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public bool State { get; set; }

        [Display(Name = "Categoría del Producto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Display(Name = "Modelo del Producto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int ModelId { get; set; }

        [ForeignKey("ModelId")]
        public Model Model { get; set; }

        public int? ParentId { get; set; }

        public virtual Product Parent { get; set; }
    }
}
