using Ecommerce.AccessData.Data;
using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.AccessData.Repository
{
    public class InventoryRepository : Repository<Inventory>, IInventory
    {
        private readonly ApplicationDbContext _context;

        public InventoryRepository(ApplicationDbContext context) : base (context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetAllDropdownList(string obj)
        {
            if(obj == "Warehouse")
            {
                return _context.Warehouses.Where(w => w.State == true).Select(w => new SelectListItem
                {
                    Text = w.Name,
                    Value = w.Id.ToString(),
                });
            }

            return null;
        }

        public void Update(Inventory inventory)
        {
            var item = _context.Inventories.FirstOrDefault(b => b.Id == inventory.Id);

            if (item != null)
            {
                item.WarehouseId = inventory.WarehouseId;
                item.DateEnd = inventory.DateEnd;
                item.State = inventory.State;   


            }
        }

    }
}
