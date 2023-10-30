using Ecommerce.Models;

namespace Ecommerce.AccessData.Repository.IRepository
{
    public interface IShoppinCar : IRepository<ShoppingCar>
    {
        void Update(ShoppingCar shoppingCar);
    }
}
