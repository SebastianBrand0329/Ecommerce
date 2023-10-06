using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;
using Ecommerce.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ModelsController : Controller
    {
        private readonly IWorkContainer _workContainer;

        public ModelsController(IWorkContainer workContainer)
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
            Model model = new();
            if (id == null)
            {
                //Create New Warehouse
                model.State = true;
                return View(model);
            }

            //Update Warehouse

            model = await _workContainer.model.Get(id.GetValueOrDefault());
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(Model model)
        {
            if (ModelState.IsValid) 
            {
                if(model.Id == 0)
                {
                    await _workContainer.model.Add(model);
                    TempData[Ds.Successful] = "Modelo creado exitosamente";
                }
                else
                {
                    _workContainer.model.Update(model);
                    TempData[Ds.Successful] = "Modelo actualizado correctamente";
                }

                await _workContainer.Saved();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }




        #region
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var item = await _workContainer.model.GetAll();
            return Json(new { data = item });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _workContainer.model.Get(id);

            if (item == null)
            {
                return Json(new { success = false, message = "Error al eliminar el modelo"});
            }

            _workContainer.model.Remove(item);
            await _workContainer.Saved();
            return Json(new { success = false, message = "Modelo eliminado exitosamente" });
        }


        [ActionName("ValidateName")] 
        public async Task<IActionResult> ValidateName(string Name, int id = 0)
        {
            bool value = false;

            var list = await _workContainer.model.GetAll();

            if (id == 0)
            {
                value = list.Any(m => m.Name.ToLower().Trim() == Name.ToLower().Trim());
            }
            else
            {
                value = list.Any(m => m.Name.ToLower().Trim() == Name.ToLower().Trim() && m.Id != id);
            }

            if (value)
            {
                return Json(new { data = true});
            }

            return Json(new { success = false, });
        }

        #endregion
    }
}
