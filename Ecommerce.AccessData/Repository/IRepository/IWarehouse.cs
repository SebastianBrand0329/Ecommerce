using Ecommerce.Models;

namespace Ecommerce.AccessData.Repository.IRepository
{
    public interface IWarehouse : IRepository<Warehouse>
    {
        void Update(Warehouse warehouse);
    }
}
