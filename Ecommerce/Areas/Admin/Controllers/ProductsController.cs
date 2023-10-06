using Ecommerce.AccessData.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IWorkContainer _workContainer;

        public ProductsController(IWorkContainer workContainer)
        {
            _workContainer = workContainer;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            return View();
        }


        #region
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var item = await _workContainer.product.GetAll(includeProperties: "Category,Model");
            return Json(new { data = item });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _workContainer.product.Get(id);

            if (item == null)
            {
                return Json(new { success = false, message = "Error al eliminar el producto" });
            }

            _workContainer.product.Remove(item);
            await _workContainer.Saved();
            return Json(new { success = true, message = "Producto eliminado exitosamente" });
        }


        [ActionName("ValidateName")]
        public async Task<IActionResult> ValidateName(string Name, int id = 0)
        {
            bool value = false;

            var list = await _workContainer.product.GetAll();

            if (id == 0)
            {
                value = list.Any(m => m.serialNumber.ToLower().Trim() == Name.ToLower().Trim());
            }
            else
            {
                value = list.Any(m => m.serialNumber.ToLower().Trim() == Name.ToLower().Trim() && m.Id != id);
            }

            if (value)
            {
                return Json(new { data = true });
            }

            return Json(new { success = false, });
        }

        #endregion
    }
}
