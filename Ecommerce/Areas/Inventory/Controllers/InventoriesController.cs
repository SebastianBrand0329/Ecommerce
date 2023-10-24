using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models.ViewModels;
using Ecommerce.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    [Authorize(Roles = Ds.Role_Admin + "," + Ds.Role_Inventory)]
    public class InventoriesController : Controller
    {
        private readonly IWorkContainer _workContainer;

        [BindProperty]
        public InventoryVM InventoryVM { get; set; }

        public InventoriesController(IWorkContainer workContainer)
        {
            _workContainer = workContainer;
        }


        public IActionResult newInventory()
        {
            InventoryVM = new InventoryVM()
            {
                Inventory = new Models.Inventory(),
                listWarehouse = _workContainer.inventory.GetAllDropdownList("Warehouse"),
            };

            InventoryVM.Inventory.State = false;
           
            //Get Id User from session

            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            InventoryVM.Inventory.UserId = claim.Value;
            InventoryVM.Inventory.DateInitial = DateTime.Now;
            InventoryVM.Inventory.DateEnd = DateTime.Now;

            return View(InventoryVM);
        }

        public IActionResult Index()
        {
            return View();
        }


        #region
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _workContainer.productWarehouse.GetAll(includeProperties: "Warehouse,Product");
            return Json(new { data = items });
        }



        #endregion
    }
}
