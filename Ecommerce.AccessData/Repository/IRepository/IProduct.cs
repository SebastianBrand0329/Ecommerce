using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.AccessData.Repository.IRepository
{
    public interface IProduct : IRepository<Product>
    {
        void Update (Product product);

        IEnumerable<SelectListItem> GetAllDropDownList(string obj);

        
    }
}
