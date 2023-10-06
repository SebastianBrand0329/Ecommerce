using Ecommerce.Models;

namespace Ecommerce.AccessData.Repository.IRepository
{
    public interface IModel : IRepository<Model>    
    {
        void Update(Model model);
    }
}
