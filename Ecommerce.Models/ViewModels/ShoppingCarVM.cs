namespace Ecommerce.Models.ViewModels
{
    public class ShoppingCarVM
    {
        public Company Company { get; set; }

        public Product Product { get; set; }

        public ShoppingCar ShoppingCar { get; set; }

        public int Stock { get; set; }

        public IEnumerable<ShoppingCar> listShoppingCar { get; set; }

        public Order Order { get; set; }
    }
}
