using Ecommerce.AccessData.Data;
using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.AccessData.Repository
{
    public class InventoryDetailsRepository : Repository<InventoryDetails>, IInventoryDetails
    {
        private readonly ApplicationDbContext _context;

        public InventoryDetailsRepository(ApplicationDbContext context) : base (context)
        {
            _context = context;
        }

        public void Update(InventoryDetails inventoryDetails)
        {
            var item = _context.InventoryDetails.FirstOrDefault(b => b.Id == inventoryDetails.Id);

            if (item != null)
            {
                item.StockPrevious = inventoryDetails.StockPrevious;
                item.Stock = inventoryDetails.Stock;  


            }
        }

    }
}
