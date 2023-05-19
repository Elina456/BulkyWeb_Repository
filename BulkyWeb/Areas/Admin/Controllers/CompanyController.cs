
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
   // [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        
        public CompanyController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
           

        }
        public IActionResult Index()
        {
            List<Company> companyList = _unitofwork.company.GetAll().ToList();
            return View(companyList);
        }
        public IActionResult Upsert(int? id)
        {
            
            //ProductVM productVM = new ()
            //{
            //    CategoryList = _unitofwork.category.GetAll().Select(u => new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.Id.ToString()
            //    }),
            //    Product = new Product()
            //};
            if (id == null || id == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                // productVM.Product = _unitofwork.product.Get(u=>u.Id==id);
                Company companyObj = _unitofwork.company.Get(u => u.Id == id);
                return View(companyObj);

            }
           
        }
        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
            
            

            if (ModelState.IsValid)
            {
                
                if(companyObj.Id==0)
                {
                    _unitofwork.company.Add(companyObj);
                    TempData["success"] = "Company Created Successfully";

                }
                else
                {
                    _unitofwork.company.update(companyObj);
                    TempData["success"] = "Company Updated Successfully";
                }
                _unitofwork.save();
               // TempData["success"] = "Company Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {

                //productVM.CategoryList = _unitofwork.category.GetAll().Select(u => new SelectListItem
                //{
                //    Text = u.Name,
                //    Value = u.Id.ToString()
                //});
                    
                
                return View(companyObj);
            }

           
        }

        
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> companyList = _unitofwork.company.GetAll().ToList();
            return Json(new {data=companyList});
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
      {
            var companyToBeDelete = _unitofwork.company.Get(u => u.Id == id);
            if(companyToBeDelete == null) 
            { 
                return Json(new {success=false, message="Error while deleting"});
            }
            
            _unitofwork.company.Remove(companyToBeDelete);
            _unitofwork.save();
           
            return Json(new { success=true,message="Delete successful" });
        }

        #endregion
    }
}
