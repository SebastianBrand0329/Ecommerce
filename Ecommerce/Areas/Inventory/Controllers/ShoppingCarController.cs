using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Ecommerce.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace Ecommerce.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class ShoppingCarController : Controller
    {
        private readonly IWorkContainer _workContainer;
        private string _webUrl;

        [BindProperty]
        public ShoppingCarVM shoppingCarVM { get; set; }

        public ShoppingCarController(IWorkContainer workContainer, IConfiguration configuration)
        {
            _workContainer = workContainer;
            _webUrl = configuration.GetValue<string>("DomainUrls:WEB_URL");
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCarVM = new ShoppingCarVM();
            shoppingCarVM.Order = new Models.Order();
            shoppingCarVM.listShoppingCar = await _workContainer.shoppinCar.GetAll(u => u.UserId == claim.Value,
                                                                                   includeProperties: "Product");

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

            if (item.Quantity == 1)
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

        public async Task<IActionResult> Remove(int carroId)
        {
            var item = await _workContainer.shoppinCar.GetFirst(c => c.Id == carroId);
            var shopping = await _workContainer.shoppinCar.GetAll(c => c.UserId == item.UserId);

            var numberProduct = shopping.Count();
            _workContainer.shoppinCar.Remove(item);
            await _workContainer.Saved();
            HttpContext.Session.SetInt32(Ds.ssShoppingCar, numberProduct - 1);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Proceed()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCarVM = new ShoppingCarVM()
            {
                Order = new Models.Order(),
                listShoppingCar = await _workContainer.shoppinCar.GetAll(c => c.UserId == claim.Value,
                                                                         includeProperties: "Product"),
                Company = await _workContainer.company.GetFirst()
            };

            shoppingCarVM.Order.TotalOrder = 0;
            shoppingCarVM.Order.User = await _workContainer.user.GetFirst(u => u.Id == claim.Value);

            foreach (var item in shoppingCarVM.listShoppingCar)
            {
                item.Price = item.Product.Price;
                shoppingCarVM.Order.TotalOrder += (item.Price * item.Quantity);
            }

            shoppingCarVM.Order.NameClient = shoppingCarVM.Order.User.Name + " " + shoppingCarVM.Order.User.LastName;
            shoppingCarVM.Order.Phone = shoppingCarVM.Order.User.PhoneNumber;
            shoppingCarVM.Order.Address = shoppingCarVM.Order.User.Address;
            shoppingCarVM.Order.Country = shoppingCarVM.Order.User.Country;
            shoppingCarVM.Order.City = shoppingCarVM.Order.User.City;

            //Stok controller

            foreach (var item in shoppingCarVM.listShoppingCar)
            {
                // Get Stock product
                var product = await _workContainer.productWarehouse.GetFirst(p => p.ProductId == item.ProductId &&
                                                                             p.WarehouseId == shoppingCarVM.Company.WarehouseId);

                if (item.Quantity > product.Stock)
                {
                    TempData[Ds.Error] = "La cantidad del Producto " + item.Product.Description + " Excede al Stock Actual (" + product.Stock + ")";
                    return RedirectToAction("Index");
                }

            }

            return View(shoppingCarVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Proceed(ShoppingCarVM shoppingCarVM)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCarVM.listShoppingCar = await _workContainer.shoppinCar.GetAll(u => u.UserId == claim.Value,
                                                                                   includeProperties: "Product");

            shoppingCarVM.Company = await _workContainer.company.GetFirst();
            shoppingCarVM.Order.TotalOrder = 0;
            shoppingCarVM.Order.UserId = claim.Value;
            shoppingCarVM.Order.OrderDate = DateTime.Now;

            foreach (var item in shoppingCarVM.listShoppingCar)
            {
                item.Price = item.Product.Price;
                shoppingCarVM.Order.TotalOrder += (item.Price * item.Quantity);
            }

            //Stock Controller

            foreach (var item in shoppingCarVM.listShoppingCar)
            {
                // Get Stock product
                var product = await _workContainer.productWarehouse.GetFirst(p => p.ProductId == item.ProductId &&
                                                                             p.WarehouseId == shoppingCarVM.Company.WarehouseId);

                if (item.Quantity > product.Stock)
                {
                    TempData[Ds.Error] = "La cantidad del Producto " + item.Product.Description + " Excede al Stock Actual (" + product.Stock + ")";
                    return RedirectToAction("Index");
                }

            }

            shoppingCarVM.Order.Stateorder = Ds.EstadoPendiente;
            shoppingCarVM.Order.statePay = Ds.EstadoPendiente;

            await _workContainer.order.Add(shoppingCarVM.Order);
            await _workContainer.Saved();

            foreach (var item in shoppingCarVM.listShoppingCar)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    OrderId = shoppingCarVM.Order.Id,
                    Price = item.Price,
                    Quantity = item.Quantity,
                };

                await _workContainer.orderDetail.Add(orderDetail);
                await _workContainer.Saved();
            }

            // Stripe

            var user = await _workContainer.user.GetFirst(u => u.Id == claim.Value);

            var options = new SessionCreateOptions
            {
                SuccessUrl = _webUrl + $"Inventory/ShoppingCar/ConfirmationOrder?id={shoppingCarVM.Order.Id}",
                CancelUrl = _webUrl + "Inventory/ShoppingCar/Index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = user.Email
            };

            //foreach (var item in shoppingCarVM.listShoppingCar)
            //{
            //    var sessionLineItem = new SessionLineItemOptions
            //    {
            //        PriceData = new SessionLineItemPriceDataOptions()
            //        {
            //            UnitAmount = (long)(item.Price * 100), //Siempre se multiplica por 100 ejemplo 20 => 200
            //            Currency = "usd",
            //            ProductData = new SessionLineItemPriceDataProductDataOptions()
            //            {
            //                Name = item.Product.Description
            //            }
            //        },
            //        Quantity = item.Quantity
            //    };

            //    options.LineItems.Add(sessionLineItem); 

            //}

            //var service = new SessionService();
            //Session session = service.Create(options);

            //_workContainer.order.UpdatePayStripe(shoppingCarVM.Order.Id, session.Id, session.PaymentIntentId);
            //await _workContainer.Saved();
            //Response.Headers.Add("Location", session.Url); // Redirect to Stripe

            //return new StatusCodeResult(303);

            foreach (var lista in shoppingCarVM.listShoppingCar)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        UnitAmount = (long)(lista.Price * 100),  // $20  => 200
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = lista.Product.Description
                        }
                    },
                    Quantity = lista.Quantity
                };
                options.LineItems.Add(sessionLineItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            _workContainer.order.UpdatePayStripe(shoppingCarVM.Order.Id, session.Id, session.PaymentIntentId);
            await _workContainer.Saved();
            Response.Headers.Add("Location", session.Url);  // Redirecciona a Stripe
            return new StatusCodeResult(303);


        }

        public async Task<IActionResult> ConfirmationOrder(int id)
        {
            var order = await _workContainer.order.GetFirst(o => o.Id == id, includeProperties:"User");
            var service = new SessionService();

            Session session = service.Get(order.SessionId);
            var shopping = await _workContainer.shoppinCar.GetAll(u => u.UserId == order.UserId);
            if (session.PaymentStatus.ToLower() =="paid")
            {
                _workContainer.order.UpdatePayStripe(id, session.Id, session.PaymentIntentId);
                _workContainer.order.UpdateState(id, Ds.EstadoAprobado, Ds.PagoEstadoAprobado);
                await _workContainer.Saved();

                //Subtract Stock 

                var company = await _workContainer.company.GetFirst();
                foreach (var item in shopping)
                {
                    var producWarehouse = new ProductWarehouse();
                    producWarehouse = await _workContainer.productWarehouse.GetFirst(b => b.ProductId == item.ProductId &&
                                                                                     b.WarehouseId == company.WarehouseId);

                    await _workContainer.kardexInventory.RegisterKardex(producWarehouse.Id, "Salida", "Venta - Orden# " + id,                                                                producWarehouse.Stock, item.Quantity, order.UserId);

                    producWarehouse.Stock -= item.Quantity;
                    await _workContainer.Saved();

                }


            }

            // Delete Shopping Car
            
            List<ShoppingCar> listShopping = shopping.ToList();
            _workContainer.shoppinCar.RemoveRange(listShopping);
            await _workContainer.Saved();

            HttpContext.Session.SetInt32(Ds.ssShoppingCar, 0);

            return View(id);
        }


    }
}
