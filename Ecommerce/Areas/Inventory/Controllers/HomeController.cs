using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;
using Ecommerce.Models.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

        public IActionResult Index(int pageNumber = 1, string search = "", string currentSearch = "")
        {
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