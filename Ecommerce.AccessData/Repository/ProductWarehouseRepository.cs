using Ecommerce.AccessData.Data;
using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.AccessData.Repository
{
    public class ProductWarehouseRepository : Repository<ProductWarehouse>, IProductWarehouse
    {
        private readonly ApplicationDbContext _context;

        public ProductWarehouseRepository(ApplicationDbContext context) : base (context)
        {
            _context = context;
        }

        public void Update(ProductWarehouse productWarehouse)
        {
            var item = _context.ProductWarehouses.FirstOrDefault(c => c.Id == productWarehouse.Id);

            if (item != null)
            { 
                item.Stock = productWarehouse.Stock;    

            }
        }

    }
}
