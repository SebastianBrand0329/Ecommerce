using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;
using Ecommerce.Models.Specifications;
using Ecommerce.Models.ViewModels;
using Ecommerce.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Versioning;
using System.Diagnostics;
using System.Security.Claims;

namespace Ecommerce.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWorkContainer _workContainer;
        [BindProperty]
        public ShoppingCarVM shoppingCarVM { get; set; }    

        public HomeController(ILogger<HomeController> logger, IWorkContainer workContainer)
        {
            _logger = logger;
            _workContainer = workContainer;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, string search = "", string currentSearch = "")
        {
            // Controller sesion
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claim != null)
            {
                var listShoppingCar = await _workContainer.shoppinCar.GetAll(c => c.UserId == claim.Value);
                var numberProduct = listShoppingCar.Count();
                HttpContext.Session.SetInt32(Ds.ssShoppingCar, numberProduct);
            }

            //Finish controller session

            if (!String.IsNullOrEmpty(search))
            {
                pageNumber = 1;
            }
            else
            {
                search = currentSearch;
            }
            ViewData["CurrentSearch"] = search;
            if(pageNumber < 1) { pageNumber = 1; }

            Parameter parameter = new Parameter()
            {
                PageNumber = pageNumber,
                PageSize = 8

            };  

            var result = _workContainer.product.GetAllPaginated(parameter);

            if(!String.IsNullOrEmpty(search)) 
            {
                result = _workContainer.product.GetAllPaginated(parameter, p => p.Description.Contains(search));
            }

            ViewData["TotalPage"] = result.MetaData.TotalPages;
            ViewData["TotalRegister"] = result.MetaData.TotalCount;
            ViewData["PageSize"] = result.MetaData.PageSize;
            ViewData["PageNumber"] = pageNumber;
            ViewData["Previo"] = "disabled"; // css desativated button
            ViewData["Next"] = "";

            if (pageNumber > 1) { ViewData["Previo"] = ""; }
            if (result.MetaData.TotalPages <= pageNumber) { ViewData["Next"] = "disabled"; }
            
            return View(result);

        }

        public async Task<IActionResult> Detail(int id)
        {
            shoppingCarVM = new ShoppingCarVM();
            shoppingCarVM.Company = await _workContainer.company.GetFirst();
            shoppingCarVM.Product = await _workContainer.product.GetFirst(p => p.Id == id,
                                                                        includeProperties: "Model,Category");

            var productWarehouse = await _workContainer.productWarehouse.GetFirst(p => p.ProductId == id &&
                                                                                  p.WarehouseId == shoppingCarVM.Company.WarehouseId);

            if (productWarehouse == null)
            {
                shoppingCarVM.Stock = 0;
            }
            else
            {
                shoppingCarVM.Stock = productWarehouse.Stock; 
            }

            shoppingCarVM.ShoppingCar = new ShoppingCar()
            {
                Product = shoppingCarVM.Product,
                ProductId = shoppingCarVM.Product.Id
            };

            return View(shoppingCarVM);
            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Detail(ShoppingCarVM shoppingCarVM)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCarVM.ShoppingCar.UserId = claim.Value;

            ShoppingCar shopping = await _workContainer.shoppinCar.GetFirst(c => c.UserId == claim.Value && 
                                                                            c.ProductId == shoppingCarVM.ShoppingCar.ProductId);

            if(shopping == null)
            {
                await _workContainer.shoppinCar.Add(shoppingCarVM.ShoppingCar);
            }
            else
            {
                shopping.Quantity += shoppingCarVM.ShoppingCar.Quantity;
                _workContainer.shoppinCar.Update(shopping);
            }
            await _workContainer.Saved();
            TempData[Ds.Successful] = "Producto Agregado Al Carro de Compra";

            //Agregate value session

            var listShoppingCar = await _workContainer.shoppinCar.GetAll(c => c.UserId == claim.Value);
            var numberProduct = listShoppingCar.Count();

            HttpContext.Session.SetInt32(Ds.ssShoppingCar, numberProduct);

            return RedirectToAction("Index");   


        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}