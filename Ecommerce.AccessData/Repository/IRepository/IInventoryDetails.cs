using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.AccessData.Repository.IRepository
{
    public interface IInventoryDetails : IRepository<InventoryDetails>
    {
        void Update(InventoryDetails inventoryDetails);

        
    }
}
