using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Ecommerce.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IWorkContainer _workContainer;

        [BindProperty]
        public OrderDetailVM  orderDetailVM { get; set; }

        public OrdersController(IWorkContainer workContainer)
        {
            _workContainer = workContainer;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int id)
        {
            orderDetailVM = new OrderDetailVM()
            {
                Order = await _workContainer.order.GetFirst(o => o.Id == id, includeProperties:"User"),
                OrderDetailList = await _workContainer.orderDetail.GetAll(d => d.OrderId == id, includeProperties:"Product")
            };

            return View(orderDetailVM); 
        }

        [Authorize(Roles = Ds.Role_Admin)]
        public async Task<IActionResult> Proceed(int id)
        {
            var order = await _workContainer.order.GetFirst(o => o.Id == id);
            order.Stateorder = Ds.EstadoProceso;
            await _workContainer.Saved();
            TempData[Ds.Successful] = "Orden Cambiado a Estado en Proceso";

            return RedirectToAction("Detail", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Ds.Role_Admin)]
        public async Task<IActionResult> SendOrder(OrderDetailVM detailVM)
        {
            var order = await _workContainer.order.GetFirst(o => o.Id == detailVM.Order.Id);
            order.Stateorder = Ds.EstadoEnviado;
            order.Carrier = detailVM.Order.Carrier;
            order.SendDate = DateTime.Now;
            order.SendNumber = detailVM.Order.SendNumber;
            await _workContainer.Saved();
            TempData[Ds.Successful] = "Orden Cambiado a Estado Enviado";

            return RedirectToAction("Detail", new { id = detailVM.Order.Id });
        }

        #region

        [HttpGet]
        public async Task<IActionResult> GetOrderList(string state)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<Order> all;

            if (User.IsInRole(Ds.Role_Admin)) // Validate Role Admin
            {
                 all = await _workContainer.order.GetAll(includeProperties: "User");
            }
            else
            {
                all = await _workContainer.order.GetAll(o => o.UserId == claim.Value ,includeProperties: "User");
            }

            //Valitade State

            switch (state)
            {
                case "approved":
                    all = all.Where(o => o.Stateorder == Ds.EstadoAprobado);
                    break;

                case "filled":
                    all = all.Where(o => o.Stateorder == Ds.EstadoEnviado);
                    break;

                default:
                    break;
            }

            return Json(new { data = all });

        }




        #endregion

    }
}
