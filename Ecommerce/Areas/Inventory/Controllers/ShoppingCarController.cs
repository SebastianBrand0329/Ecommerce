using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models.ViewModels;
using Ecommerce.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class ShoppingCarController : Controller
    {
        private readonly IWorkContainer _workContainer;
        [BindProperty]
        public ShoppingCarVM shoppingCarVM { get; set; }

        public ShoppingCarController(IWorkContainer workContainer)
        {
            _workContainer = workContainer;
        }

        [Authorize]
        public  async Task<IActionResult> Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCarVM = new ShoppingCarVM();
            shoppingCarVM.Order = new Models.Order();
            shoppingCarVM.listShoppingCar = await _workContainer.shoppinCar.GetAll(u => u.UserId == claim.Value,
                                                                                   includeProperties:"Product");

            shoppingCarVM.Order.TotalOrder = 0;
            shoppingCarVM.Order.UserId = claim.Value;

            foreach (var item in shoppingCarVM.listShoppingCar)
            {
                item.Price = item.Product.Price;
                shoppingCarVM.Order.TotalOrder += (item.Price * item.Quantity); 
            }

            return View(shoppingCarVM);

        }

        public async Task<IActionResult> Add(int carroId)
        {
            var item = await _workContainer.shoppinCar.GetFirst(c => c.Id == carroId);

            item.Quantity += 1;
            await _workContainer.Saved();
            return RedirectToAction("Index");   
            
        }

        public async Task<IActionResult> Subtract(int carroId)
        {
            var item = await _workContainer.shoppinCar.GetFirst(c => c.Id == carroId);

            if(item.Quantity == 1)
            {
                //Remove and update

                var shopping = await _workContainer.shoppinCar.GetAll(c => c.UserId == item.UserId);

                var numberProduct = shopping.Count();
                _workContainer.shoppinCar.Remove(item);
                HttpContext.Session.SetInt32(Ds.ssShoppingCar, numberProduct - 1);
            }
            else
            {
                item.Quantity -= 1;
            }
            await _workContainer.Saved();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult>Remove(int carroId)
        {
            var item = await _workContainer.shoppinCar.GetFirst(c => c.Id == carroId);
            var shopping = await _workContainer.shoppinCar.GetAll(c => c.UserId == item.UserId);

            var numberProduct = shopping.Count();
            _workContainer.shoppinCar.Remove(item);
            await _workContainer.Saved();
            HttpContext.Session.SetInt32(Ds.ssShoppingCar, numberProduct - 1);
            return RedirectToAction("Index");
        }


    }
}
