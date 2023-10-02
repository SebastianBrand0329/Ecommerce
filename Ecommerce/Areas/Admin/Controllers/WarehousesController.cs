using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;

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




        #region API

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var all = await _workContainer.warehouse.GetAll();
            return Json(new { data = all }); 
        }

        #endregion
    }
}
