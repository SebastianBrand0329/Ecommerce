using Ecommerce.Models;

namespace Ecommerce.AccessData.Repository.IRepository
{
    public interface IProduct : IRepository<Product>
    {
        void Update (Product product);    
    }
}
