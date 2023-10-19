using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }

        public IEnumerable<SelectListItem> categoryList { get; set; }

        public IEnumerable<SelectListItem> modelList { get; set; }

        public IEnumerable<SelectListItem> parentList { get; set; }
    }
}
