using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Ecommerce.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Ds.Role_Admin + "," + Ds.Role_Inventory)]
    public class ProductsController : Controller
    {
        private readonly IWorkContainer _workContainer;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IWorkContainer workContainer, IWebHostEnvironment webHostEnvironment)
        {
            _workContainer = workContainer;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new Product(),
                categoryList = _workContainer.product.GetAllDropDownList("Category"),
                modelList = _workContainer.product.GetAllDropDownList("Model"),
                parentList = _workContainer.product.GetAllDropDownList("Product")
            };

            if (id == null)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = await _workContainer.product.Get(id.GetValueOrDefault());

                if (productVM.Product == null)
                {
                    return NotFound();
                }

                return View(productVM);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productVM.Product.Id == 0)
                {
                    //Create
                    string upload = webRootPath + Ds.RouteImage;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = fileName + extension;
                    await _workContainer.product.Add(productVM.Product);
                }
                else
                {
                    //Update
                    var objProduct = await _workContainer.product.GetFirst(p => p.Id == productVM.Product.Id, isTracking: false);

                    if (files.Count() > 0)
                    {
                        string upload = webRootPath + Ds.RouteImage;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        //Delete Image
                        var delete = Path.Combine(upload, objProduct.ImageUrl);
                        if (System.IO.File.Exists(delete))
                        {
                            System.IO.File.Delete(delete);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.ImageUrl = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.ImageUrl = objProduct.ImageUrl;
                    }

                    _workContainer.product.Update(productVM.Product);  
                }

                TempData[Ds.Successful] = "Transacción Exitosa!";
                await _workContainer.Saved();
                return View("Index");
            }

            productVM.categoryList = _workContainer.product.GetAllDropDownList("Category");
            productVM.modelList = _workContainer.product.GetAllDropDownList("Model");
            productVM.parentList = _workContainer.product.GetAllDropDownList("Product");

            return View(productVM);
        }




        #region
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var item = await _workContainer.product.GetAll(includeProperties: "Category,Model");
            return Json(new { data = item });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _workContainer.product.Get(id);

            if (item == null)
            {
                return Json(new { success = false, message = "Error al eliminar el producto" });
            }

            //Remove Image
            string upload = _webHostEnvironment.WebRootPath + Ds.RouteImage;
            var file = Path.Combine(upload, item.ImageUrl);

            if (System.IO.File.Exists(file))
            {
                System.IO.File.Delete(file);    
            }

            _workContainer.product.Remove(item);
            await _workContainer.Saved();
            return Json(new { success = true, message = "Producto eliminado exitosamente" });
        }


        [ActionName("ValidateName")]
        public async Task<IActionResult> ValidateName(string Name, int id = 0)
        {
            bool value = false;

            var list = await _workContainer.product.GetAll();

            if (id == 0)
            {
                value = list.Any(m => m.serialNumber.ToLower().Trim() == Name.ToLower().Trim());
            }
            else
            {
                value = list.Any(m => m.serialNumber.ToLower().Trim() == Name.ToLower().Trim() && m.Id != id);
            }

            if (value)
            {
                return Json(new { data = true });
            }

            return Json(new { success = false, });
        }

        #endregion
    }
}


