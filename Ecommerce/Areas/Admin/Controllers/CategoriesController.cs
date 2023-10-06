using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;
using Ecommerce.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IWorkContainer _workContainer;

        public CategoriesController(IWorkContainer workContainer)
        {
            _workContainer = workContainer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            Category category = new ();
            if (id == null)
            {
                //Create New Warehouse
                category.State = true;
                return View(category);
            }

            //Update Warehouse

            category = await _workContainer.category.Get(id.GetValueOrDefault());
            if (category == null)
            {
                return NotFound();
            }

            return View(category); 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                {
                    await _workContainer.category.Add(category);
                    TempData[Ds.Successful] = "Categoría creada éxisotamente";
                }
                else
                {
                    _workContainer.category.Update(category);
                    TempData[Ds.Successful] = "Categoría actualizada éxisotamente";
                }

                await _workContainer.Saved();
                return RedirectToAction(nameof(Index));   
            }
            TempData[Ds.Error] = "Error al guardar la Bodega";
            return View(category);
        }




        #region API

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var all = await _workContainer.category.GetAll();
            return Json(new { data = all }); 
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id) 
        {
            var item = await _workContainer.category.Get(id);

            if (item == null)
            {
                return Json(new {success = false, message= "Error al eliminar la Categoría" });
            }

            _workContainer.category.Remove(item);
            await _workContainer.Saved();
            return Json(new { success = true, message= "Categoría Eliminada exitosamente"});
        }

        [ActionName("ValidateName")]
        public async Task<IActionResult> ValidateName(string Name, int id = 0)
        {
            bool value = false;

            var list = await _workContainer.category.GetAll();

            if (id == 0)
            {
                value = list.Any(w => w.Name.ToLower().Trim() == Name.ToLower().Trim());
            }
            else
            {
                value = list.Any(w => w.Name.ToLower().Trim() == Name.ToLower().Trim() && w.Id != id);
            }
            if (value)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });

        }

        #endregion
    }
}
