using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models.ViewModels;
using Ecommerce.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Ds.Role_Admin)]
    public class CompanysController : Controller
    {
        private readonly IWorkContainer _workContainer;

        public CompanysController(IWorkContainer workContainer)
        {
            _workContainer = workContainer;
        }

        public async Task<IActionResult> Upsert()
        {
            CompanyVM companyVM = new CompanyVM()
            {
                Company = new Models.Company(),
                listWarehouse =  _workContainer.inventory.GetAllDropdownList("Warehouse")
            };

            companyVM.Company = await _workContainer.company.GetFirst();

            if (companyVM.Company == null)
            {
                companyVM.Company = new Models.Company();
            }
            

            return View(companyVM); 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Upsert(CompanyVM companyVM)
        {
            if (ModelState.IsValid)
            {
                TempData[Ds.Successful] = "Compañía Grabada Exitosamente";
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

                if (companyVM.Company.Id == 0) // Create Company
                {
                    companyVM.Company.UserCreateId = claim.Value;
                    companyVM.Company.UserUpdateId = claim.Value;
                    companyVM.Company.CreationDate = DateTime.Now;
                    companyVM.Company.UpdateDate = DateTime.Now;
                    await _workContainer.company.Add(companyVM.Company);
                }
                else //Update
                {
                    companyVM.Company.UserCreateId = claim.Value;
                    companyVM.Company.UserUpdateId = claim.Value;
                    companyVM.Company.UpdateDate = DateTime.Now;
                    _workContainer.company.Update(companyVM.Company); 
                }

                await _workContainer.Saved();

                return RedirectToAction("Index", "Home", new { area = "Inventory"});
            }
            TempData[Ds.Error] = "Error al grabar compañía";

            return View(companyVM); 
        }


    }
}
