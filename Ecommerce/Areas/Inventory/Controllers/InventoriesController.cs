using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Ecommerce.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;
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


        public IActionResult Index()
        {
            return View();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> newInventory(InventoryVM inventoryVM)
        {
            if (ModelState.IsValid)
            {
                inventoryVM.Inventory.DateInitial = DateTime.Now;
                inventoryVM.Inventory.DateEnd = DateTime.Now;
                await _workContainer.inventory.Add(inventoryVM.Inventory);
                await _workContainer.Saved();

                return RedirectToAction("InventoryDetails", new { id = inventoryVM.Inventory.Id });
            }
            inventoryVM.listWarehouse = _workContainer.inventory.GetAllDropdownList("Warehouse");

            return View(inventoryVM);   
        }

        public async Task<IActionResult> InventoryDetails(int id)
        {
            InventoryVM = new InventoryVM();

            InventoryVM.Inventory = await _workContainer.inventory.GetFirst(i => i.Id == id, includeProperties: "Warehouse");
            InventoryVM.ListInventoryDetails = await _workContainer.inventoryDetails.GetAll
                (d => d.InventoryId == id, includeProperties: "Product,Product.Model");

            return View(InventoryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>InventoryDetails(int InventoryId, int productId, int quantityId)
        {
             InventoryVM = new InventoryVM();
            InventoryVM.Inventory = await _workContainer.inventory.GetFirst(i => i.Id == InventoryId);

            var item = await _workContainer.productWarehouse.GetFirst(w => w.ProductId == productId &&
                                                                      w.WarehouseId == InventoryVM.Inventory.WarehouseId);

            var detail = await _workContainer.inventoryDetails.GetFirst(d => d.InventoryId == InventoryId && d.ProductId == productId);

            if(detail == null)
            {
                InventoryVM.InventoryDetails = new InventoryDetails();
                InventoryVM.InventoryDetails.ProductId = productId;
                InventoryVM.InventoryDetails.InventoryId = InventoryId;  

                if(item != null)
                {
                    InventoryVM.InventoryDetails.StockPrevious = item.Stock;
                }
                else
                {
                    InventoryVM.InventoryDetails.StockPrevious = 0;
                }

                InventoryVM.InventoryDetails.Stock = quantityId;
                await _workContainer.inventoryDetails.Add(InventoryVM.InventoryDetails);
                await _workContainer.Saved();
            }
            else
            {
                detail.Stock += quantityId;
                await _workContainer.Saved();
            }

            return RedirectToAction("InventoryDetails", new { id = InventoryId });
        }

        public async Task<IActionResult>addAmount(int id)// id of detail
        {
            InventoryVM = new InventoryVM();

            var detail = await _workContainer.inventoryDetails.Get(id);
            InventoryVM.Inventory = await _workContainer.inventory.Get(detail.Id);

            detail.Stock += 1;
            await _workContainer.Saved();

            return RedirectToAction("InventoryDetails", new { id = detail.InventoryId });
        }

        public async Task<IActionResult> subtractAmount(int id)// id of detail
        {
            InventoryVM = new InventoryVM();

            var detail = await _workContainer.inventoryDetails.Get(id);
            InventoryVM.Inventory = await _workContainer.inventory.Get(detail.Id);

            if (detail.Stock == 1)
            {
                _workContainer.inventoryDetails.Remove(detail);
                await _workContainer.Saved();

            }
            else
            {
                detail.Stock -= 1;
                await _workContainer.Saved();
            }

            return RedirectToAction("InventoryDetails", new { id = detail.InventoryId });
        }


        #region
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _workContainer.productWarehouse.GetAll(includeProperties: "Warehouse,Product");
            return Json(new { data = items });
        }



        [HttpGet]
        public async Task<IActionResult>SearchProduct(string term)
        {
            if (!string.IsNullOrEmpty(term))
            {
                var items = await _workContainer.product.GetAll(p => p.State == true);
                var data = items.Where(x => x.serialNumber.Contains(term, StringComparison.OrdinalIgnoreCase) || 
                                       x.Description.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();

                return Ok(data);
            }

            return Ok();
        }



        #endregion
    }
}
