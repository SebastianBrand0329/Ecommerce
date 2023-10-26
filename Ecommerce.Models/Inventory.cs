using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class Inventory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public DateTime DateInitial { get; set; }

        [Required]
        public DateTime DateEnd { get; set; }

        [Display(Name ="Bodega")]
        [Required(ErrorMessage ="El campo {0} es obligatorio")]
        public int WarehouseId { get; set; }

        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; set; }

        [Required]
        public bool State { get; set; }

    }
}
