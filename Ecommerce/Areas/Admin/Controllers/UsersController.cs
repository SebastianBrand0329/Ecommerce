using Ecommerce.AccessData.Data;
using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Ds.Role_Admin)]
    public class UsersController : Controller
    {
        private readonly IWorkContainer _workContainer;
        private readonly ApplicationDbContext _context;

        public UsersController(IWorkContainer workContainer, ApplicationDbContext context)
        {
            _workContainer = workContainer;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }



        #region Api

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _workContainer.user.GetAll();
            var userRole = await _context.UserRoles.ToListAsync();
            var role = await _context.Roles.ToListAsync();

            foreach (var user in users) 
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = role.FirstOrDefault(u => u.Id == roleId).Name;
            }

            return Json(new { data = users });
        }

        [HttpPost]
        public async Task<IActionResult> unlock([FromBody] string id)
        {
            var user = await _workContainer.user.GetFirst(u => u.Id == id);

            if(user == null)
            {
                return Json(new { success = false, message = "Error de Usuario" });
            }
            if(user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
            {
                // User Lock
                user.LockoutEnd = DateTime.Now;
            }
            else
            {
                user.LockoutEnd = DateTime.Now.AddYears(1000);
            }

            await _workContainer.Saved();

            return Json(new { success = true, message = "Operación Exitosa" });
        }

        #endregion
    }
}
