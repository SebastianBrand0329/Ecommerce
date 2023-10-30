using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Ecommerce.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using NuGet.Versioning;
using Rotativa.AspNetCore;
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

            //Finish Get User
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
        public async Task<IActionResult> InventoryDetails(int InventoryId, int productId, int quantityId)
        {
            InventoryVM = new InventoryVM();
            InventoryVM.Inventory = await _workContainer.inventory.GetFirst(i => i.Id == InventoryId);

            var item = await _workContainer.productWarehouse.GetFirst(w => w.ProductId == productId &&
                                                                      w.WarehouseId == InventoryVM.Inventory.WarehouseId);

            var detail = await _workContainer.inventoryDetails.GetFirst(d => d.InventoryId == InventoryId && d.ProductId == productId);

            if (detail == null)
            {
                InventoryVM.InventoryDetails = new InventoryDetails();
                InventoryVM.InventoryDetails.ProductId = productId;
                InventoryVM.InventoryDetails.InventoryId = InventoryId;

                if (item != null)
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

        public async Task<IActionResult> addAmount(int id)// id of detail
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


        //public async Task<IActionResult> GenerateStock(int Id)
        //{
        //    var inventory = await _workContainer.inventory.Get(Id);

        //    var detailList = await _workContainer.inventoryDetails.GetAll(d => d.InventoryId == Id);

        //    //Get Id User from session

        //    var claimIdentity = (ClaimsIdentity)User.Identity;
        //    var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

        //    //Finish Get User

        //    foreach (var item in detailList)
        //    {
        //        var productWarehouse = new ProductWarehouse();
        //        productWarehouse = await _workContainer.productWarehouse.GetFirst(p => p.ProductId == item.ProductId &&
        //                                                                 p.WarehouseId == inventory.WarehouseId);

        //        if (productWarehouse != null) // Inventory exists in BD, update quantity
        //        {
        //            await _workContainer.kardexInventory.RegisterKardex(productWarehouse.Id, "Entrada", "Registro de Inventario",
        //                                                                         productWarehouse.Stock, item.Stock, claim.Value);
        //            productWarehouse.Stock += item.Stock;
        //            await _workContainer.Saved();
        //        }
        //        else
        //        {
        //            // Inventory not exist in bd, cretae 
        //            productWarehouse = new ProductWarehouse();
        //            productWarehouse.WarehouseId = inventory.WarehouseId;
        //            productWarehouse.ProductId = item.ProductId;
        //            productWarehouse.Stock = item.Stock;
        //            await _workContainer.productWarehouse.Add(productWarehouse);
        //            await _workContainer.Saved();
        //            await _workContainer.kardexInventory.RegisterKardex(productWarehouse.Id, "Salida", "Inventario Inicial",
        //                                            0, item.Stock, claim.Value);
        //        }

        //    }

        //    //Update 
        //    inventory.State = true;
        //    inventory.DateEnd = DateTime.Now;
        //    await _workContainer.Saved();

        //    TempData[Ds.Successful] = "Stock Generado Con Exito";

        //    return RedirectToAction("Index");
        //}

        public async Task<IActionResult> GenerateStock(int id)
        {
            var inventory = await _workContainer.inventory.Get(id);
            var detailList = await _workContainer.inventoryDetails.GetAll(d => d.InventoryId == id);
            // Obtener el Id del Usuario desde la sesion
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            foreach (var item in detailList)
            {
                var productWarehouse = new ProductWarehouse();
                productWarehouse = await _workContainer.productWarehouse.GetFirst(b => b.ProductId == item.ProductId && b.WarehouseId == inventory.WarehouseId);
                if (productWarehouse != null) //  El registro de Stock existe, hay que actualizar las cantidades
                {
                    await _workContainer.kardexInventory.RegisterKardex(productWarehouse.Id, "Entrada", "Registro de Inventario",
                                                                          productWarehouse.Stock, item.Stock, claim.Value);
                    productWarehouse.Stock += item.Stock;
                    await _workContainer.Saved();

                }
                else  // Registro de Stock no existe, hay que crearlo
                {
                    productWarehouse = new ProductWarehouse();
                    productWarehouse.WarehouseId = inventory.WarehouseId;
                    productWarehouse.ProductId = item.ProductId;
                    productWarehouse.Stock = item.Stock;
                    await _workContainer.productWarehouse.Add(productWarehouse);
                    await _workContainer.Saved();
                    await _workContainer.kardexInventory.RegisterKardex(productWarehouse.Id, "Entrada", "Inventario Inicial", 0, item.Stock, claim.Value);
                }

            }
            // Actualizar la Cabecera de Inventario
            inventory.State = true;
            inventory.DateInitial = DateTime.Now;
            await _workContainer.Saved();
            TempData[Ds.Successful] = "Stock Generado con Exito";
            return RedirectToAction("Index");

        }


        public IActionResult KardexProduct()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult KardexProduct(string fechaInicioId, string fechaFinalId, int productoId)
        {
            return RedirectToAction("KardexProductResult", new { fechaInicioId, fechaFinalId, productoId });
        }

        public async Task<IActionResult> KardexProductResult(string fechaInicioId, string fechaFinalId, int productoId)
        {
            var kardexInventoryVM = new KardexInventoryVM();
            kardexInventoryVM.Product = new Product();
            kardexInventoryVM.Product = await _workContainer.product.Get(productoId);

            kardexInventoryVM.DateInitial = DateTime.Parse(fechaInicioId);
            kardexInventoryVM.DateEnd = DateTime.Parse(fechaFinalId).AddHours(23).AddMinutes(59);

            kardexInventoryVM.ListKardexInventory = await _workContainer.kardexInventory.GetAll(k => k.ProductWarehouse.ProductId == productoId && (k.DateRegister >= kardexInventoryVM.DateInitial
                         && k.DateRegister <= kardexInventoryVM.DateEnd),
                         includeProperties: "ProductWarehouse,ProductWarehouse.Product,ProductWarehouse.Warehouse",
                        orderBy: o => o.OrderBy(o => o.DateRegister));
            return View(kardexInventoryVM);
        }

        public async Task<IActionResult> PrintReport(string DateInitial, string DateEnd, int Id)
        {
            var kardexInventoryVM = new KardexInventoryVM();
            kardexInventoryVM.Product = new Product();
            kardexInventoryVM.Product = await _workContainer.product.Get(Id);

            kardexInventoryVM.DateInitial =  DateTime.Parse(DateInitial);
            kardexInventoryVM.DateEnd = DateTime.Parse(DateEnd);

            kardexInventoryVM.ListKardexInventory = await _workContainer.kardexInventory.GetAll(k => k.ProductWarehouse.ProductId == Id && (k.DateRegister >= kardexInventoryVM.DateInitial
                         && k.DateRegister <= kardexInventoryVM.DateEnd),
                         includeProperties: "ProductWarehouse,ProductWarehouse.Product,ProductWarehouse.Warehouse",
                        orderBy: o => o.OrderBy(o => o.DateRegister));


            return new ViewAsPdf("PrintReport", kardexInventoryVM)
            {
                FileName = "KardexProduct.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 12"
            };
        }




        #region
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _workContainer.productWarehouse.GetAll(includeProperties: "Warehouse,Product");
            return Json(new { data = items });
        }



        [HttpGet]
        public async Task<IActionResult> SearchProduct(string term)
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
