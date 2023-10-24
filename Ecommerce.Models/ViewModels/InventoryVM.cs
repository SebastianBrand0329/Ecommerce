using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.Models.ViewModels
{
    public class InventoryVM
    {
        public Inventory Inventory { get; set; }

        public InventoryDetails InventoryDetails { get; set; }

        public List<InventoryDetails> ListInventoryDetails { get; set; }

        public IEnumerable<SelectListItem> listWarehouse { get; set; }
    }
}
