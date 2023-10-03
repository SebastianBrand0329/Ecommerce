using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;
using Ecommerce.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WarehousesController : Controller
    {
        private readonly IWorkContainer _workContainer;

        public WarehousesController(IWorkContainer workContainer)
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
            Warehouse warehouse = new Warehouse();
            if (id == null)
            {
                //Create New Warehouse
                warehouse.State = true;
                return View(warehouse);
            }

            //Update Warehouse

            warehouse = await _workContainer.warehouse.Get(id.GetValueOrDefault());
            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse); 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                if (warehouse.Id == 0)
                {
                    await _workContainer.warehouse.Add(warehouse);
                    TempData[Ds.Successful] = "Bodega creada éxisotamente";
                }
                else
                {
                    _workContainer.warehouse.Update(warehouse);
                    TempData[Ds.Successful] = "Bodega actualizada éxisotamente";
                }

                await _workContainer.Saved();
                return RedirectToAction(nameof(Index));   
            }
            TempData[Ds.Error] = "Error al guardar la Bodega";
            return View(warehouse);
        }




        #region API

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var all = await _workContainer.warehouse.GetAll();
            return Json(new { data = all }); 
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id) 
        {
            var item = await _workContainer.warehouse.Get(id);

            if (item == null)
            {
                return Json(new {success = false, message= "Error al eliminar la Bodega" });
            }

            _workContainer.warehouse.Remove(item);
            await _workContainer.Saved();
            return Json(new { success = true, message= "Bodega Eliminada exitosamente"});
        }

        [ActionName("ValidateName")]
        public async Task<IActionResult> ValidateName(string Name, int id = 0)
        {
            bool value = false;

            var list = await _workContainer.warehouse.GetAll();

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
