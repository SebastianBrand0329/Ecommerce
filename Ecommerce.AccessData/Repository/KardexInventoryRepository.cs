using Ecommerce.AccessData.Data;
using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;

namespace Ecommerce.AccessData.Repository
{
    public class KardexInventoryRepository : Repository<KardexInventory>, IKardexInventory
    {
        private readonly ApplicationDbContext _context;

        public KardexInventoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
