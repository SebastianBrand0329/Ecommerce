using Ecommerce.AccessData.Data;
using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

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


        public IEnumerable<SelectListItem> GetAllDropDownList(string obj)
        {
            if (obj == "Category")
            {
                return _context.Categories.Where(c => c.State == true).Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }); ;
            }

            if (obj == "Model")
            {
                return _context.Models.Where(m => m.State == true).Select(m => new SelectListItem
                {
                    Text = m.Name,
                    Value = m.Id.ToString()
                }); ;
            }

            if (obj == "Product")
            {
                return _context.Products.Where(p => p.State == true).Select(p => new SelectListItem
                {
                    Text = p.Description,
                    Value = p.Id.ToString()
                }); ;
            }

            return null;
        }


    }
}
