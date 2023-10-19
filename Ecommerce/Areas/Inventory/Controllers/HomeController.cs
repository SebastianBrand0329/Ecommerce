using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Ecommerce.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWorkContainer _workContainer;

        public HomeController(ILogger<HomeController> logger, IWorkContainer workContainer)
        {
            _logger = logger;
            _workContainer = workContainer;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> produtc = await _workContainer.product.GetAll(); 
            return View(produtc);
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