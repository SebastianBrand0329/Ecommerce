using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.AccessData.Repository.IRepository
{
    public interface IKardexInventory : IRepository<KardexInventory>
    {

        Task RegisterKardex(int productWarehouseId, string type, string detail, int stockPrevious, int quantity, string userId);

    }
}
