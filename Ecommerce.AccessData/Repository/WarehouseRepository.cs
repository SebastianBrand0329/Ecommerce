using Ecommerce.AccessData.Data;
using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;

namespace Ecommerce.AccessData.Repository
{
    public class WarehouseRepository : Repository<Warehouse>, IWarehouse
    {
        private readonly ApplicationDbContext _context;

        public WarehouseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Warehouse warehouse)
        {
            var item = _context.Warehouses.FirstOrDefault(w => w.Id == warehouse.Id);

            if (item != null)
            {
                //item.Name = warehouse.Name;
                //item.Description = warehouse.Description;
                //item.State = warehouse.State;
                _context.Warehouses.Update(warehouse);   
                _context.SaveChanges();
            }
        }
    }
}
