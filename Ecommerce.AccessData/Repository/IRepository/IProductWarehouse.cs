using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.AccessData.Repository.IRepository
{
    public interface IProductWarehouse : IRepository<ProductWarehouse>
    {
        void Update(ProductWarehouse productWarehouse);

        
    }
}
