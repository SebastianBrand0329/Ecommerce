using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.AccessData.Repository.IRepository
{
    public interface IInventory : IRepository<Inventory>
    {
        void Update(Inventory inventory);

        IEnumerable<SelectListItem> GetAllDropdownList(string obj);    

        
    }
}
