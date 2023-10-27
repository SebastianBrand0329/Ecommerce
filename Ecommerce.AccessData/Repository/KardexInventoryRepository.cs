using Ecommerce.AccessData.Data;
using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.AccessData.Repository
{
    public class KardexInventoryRepository : Repository<KardexInventory>, IKardexInventory
    {
        private readonly ApplicationDbContext _context;

        public KardexInventoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task RegisterKardex(int productWarehouseId, string type, string detail, int stockPrevious, int quantity, string userId)
        {
           var producWarehouse = await _context.ProductWarehouses.Include(b => b.Product).FirstOrDefaultAsync(b => b.Id == productWarehouseId);
            if(type == "Entrada")
            {
                KardexInventory kardex = new KardexInventory();
                kardex.ProductWarehouseId = productWarehouseId;
                kardex.Type = type; 
                kardex.Detail = detail; 
                kardex.StockPrevious = stockPrevious;   
                kardex.Quantity = quantity;
                kardex.Cost = producWarehouse.Product.Cost;
                kardex.Stock = stockPrevious + quantity;
                kardex.Total = kardex.Stock * kardex.Cost;
                kardex.UserId = userId;
                kardex.DateRegister = DateTime.Now; 

                await _context.KardexInventories.AddAsync(kardex);
                await _context.SaveChangesAsync();  

            }

            if (type == "Salida")
            {
                KardexInventory kardex = new KardexInventory();
                kardex.ProductWarehouseId = productWarehouseId;
                kardex.Type = type;
                kardex.Detail = detail;
                kardex.StockPrevious = stockPrevious;
                kardex.Quantity = quantity;
                kardex.Cost = producWarehouse.Product.Cost;
                kardex.Stock = stockPrevious - quantity;
                kardex.Total = kardex.Stock * kardex.Cost;
                kardex.UserId = userId;
                kardex.DateRegister = DateTime.Now;

                await _context.KardexInventories.AddAsync(kardex);
                await _context.SaveChangesAsync();

            }

        }
    }
}
