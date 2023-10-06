using Ecommerce.AccessData.Data;
using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;

namespace Ecommerce.AccessData.Repository
{
    public class ProductRepository : Repository<Product>, IProduct
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base (context)
        {
            _context = context;
        }

        public void Update(Product product)
        {
            var item = _context.Products.FirstOrDefault(c => c.Id == product.Id);

            if (item != null)
            {
                if(product.ImageUrl != null) 
                {
                   item.ImageUrl = product.ImageUrl;    
                }

                item.serialNumber = product.serialNumber;
                item.Description = product.Description; 
                item.Price = product.Price;
                item.Cost = product.Cost;   
                item.State = product.State;
                item.CategoryId = product.CategoryId;   
                item.ModelId = product.ModelId; 

                
            }
        }

       
    }
}
