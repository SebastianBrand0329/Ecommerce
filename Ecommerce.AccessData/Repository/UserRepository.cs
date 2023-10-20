using Ecommerce.AccessData.Data;
using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;

namespace Ecommerce.AccessData.Repository
{
    public class UserRepository : Repository<User>, IUser
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
