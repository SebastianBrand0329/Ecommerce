using Ecommerce.AccessData.Data;
using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;

namespace Ecommerce.AccessData.Repository
{
    public class CompanyRepository : Repository<Company>, ICompany
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Company company)
        {
            var item = _context.Companys.FirstOrDefault(w => w.Id == company.Id);

            if (item != null)
            {
                item.Name = company.Name;
                item.Description = company.Description;
                item.Country = company.Country;
                item.City = company.City;   
                item.Address = company.Address;
                item.Phone = company.Phone;
                item.WarehouseId = company.WarehouseId; 
                item.UserCreateId = company.UserCreateId;
                item.UserUpdateId = company.UserUpdateId;


                //_context.Warehouses.Update(warehouse);   
                //_context.SaveChanges();
            }
        }
    }
}
