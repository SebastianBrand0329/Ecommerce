namespace Ecommerce.Models.ViewModels
{
    public class OrderDetailVM
    {
        public Order Order { get; set; }

        public Company Company { get; set; }

        public IEnumerable<OrderDetail> OrderDetailList { get; set; }
    }
}
