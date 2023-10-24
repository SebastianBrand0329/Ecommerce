using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class KardexInventory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductWarehouseId { get; set; }

        [ForeignKey("ProductWarehouseId")]
        public ProductWarehouse ProductWarehouse { get; set; }

        [Required]
        [MaxLength(100)]
        public string Type { get; set; } //Output or Input

        [Display(Name ="Detalle")]
        [Required]
        public string Detail { get; set; }

        [Required]
        public int StockPrevious { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double Cost { get; set; }

        [Required]
        public int Stock { get; set; }

        public double Total { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public DateTime DateRegister { get; set; }
    }
}
