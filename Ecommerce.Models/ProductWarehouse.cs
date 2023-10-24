using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class ProductWarehouse
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Bodega")]
        [Required]
        public int WarehouseId { get; set; }

        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; set; }

        [Display(Name ="Producto")]
        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Display(Name ="Cantidad")]
        [Required]
        public int Stock { get; set; }
    }
}
