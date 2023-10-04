using Ecommerce.AccessData.Data;
using Ecommerce.AccessData.Repository.IRepository;

namespace Ecommerce.AccessData.Repository
{
    public class WorkContainer : IWorkContainer
    {
        private readonly ApplicationDbContext _context;
        public IWarehouse warehouse { get; private set; }
        public ICategory category { get; private set; }


        public WorkContainer(ApplicationDbContext context)
        {
            _context = context;
            warehouse = new WarehouseRepository(_context);
            category = new CategoryRepository(_context);    
        }


        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task Saved()
        {
            await _context.SaveChangesAsync();  
        }
    }
}
