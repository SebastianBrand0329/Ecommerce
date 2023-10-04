using Ecommerce.AccessData.Data;
using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;

namespace Ecommerce.AccessData.Repository
{
    public class CategoryRepository : Repository<Category>, ICategory
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base (context)
        {
            _context = context;
        }

        public void Update(Category category)
        {
            var item = _context.Categories.FirstOrDefault(c => c.Id == category.Id);

            if (item != null)
            {
                item.Name = category.Name;  
                item.Description = category.Description;    
                item.State = category.State;    
            }
        }
    }
}
