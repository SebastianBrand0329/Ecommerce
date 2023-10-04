using Ecommerce.Models;

namespace Ecommerce.AccessData.Repository.IRepository
{
    public interface ICategory : IRepository<Category>
    {
        void Update (Category category);    
    }
}
