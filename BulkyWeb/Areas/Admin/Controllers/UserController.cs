
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
   [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        
        public UserController(ApplicationDbContext db)
        {
            _db = db;
           

        }
        public IActionResult Index()
        {
            return View();
        }
        
        
        
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> userList = _db.applicationUsers.Include(u=>u.Company).ToList();
            return Json(new {data=userList});
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
      {
           
           
            return Json(new { success=true,message="Delete successful" });
        }

        #endregion
    }
}
