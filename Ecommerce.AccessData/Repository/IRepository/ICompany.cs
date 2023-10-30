using Ecommerce.Models;

namespace Ecommerce.AccessData.Repository.IRepository
{
    public interface ICompany : IRepository<Company>
    {
        void Update(Company company);
    }
}
