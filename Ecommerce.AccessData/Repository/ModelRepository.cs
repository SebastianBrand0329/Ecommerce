using Ecommerce.AccessData.Data;
using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;

namespace Ecommerce.AccessData.Repository
{
    public class ModelRepository : Repository<Model>, IModel
    {
        private readonly ApplicationDbContext _context;

        public ModelRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }


        public void Update(Model model)
        {
            var item = _context.Categories.FirstOrDefault(m => m.Id == model.Id);

            if (item != null)
            {
                item.Name = model.Name;
                item.State = model.State;
            }
        }
    }
}
