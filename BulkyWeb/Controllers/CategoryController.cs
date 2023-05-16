﻿using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
			_db = db;
				
        }
        public IActionResult Index()
		{
			List<Category> categoryList = _db.Categories.ToList();
			return View(categoryList);
		}
		public IActionResult Create() 
		{
			return View();
		}
		[HttpPost]
        public IActionResult Create(Category OBJ)
        {
			if(OBJ.Name == OBJ.DisplayOrder.ToString())
			{
				ModelState.AddModelError("name", "DISPLAYORDER CANNOT MATCH THE NAME");
			}
			if(ModelState.IsValid)
			{
                _db.Categories.Add(OBJ);
                _db.SaveChanges();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }
			
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id==0) 
            {
                return NotFound();
            }
            Category? categoryFromDb = _db.Categories.Find(id);
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();
            if (categoryFromDb == null) 
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category OBJ)
        {
            
            if (ModelState.IsValid)
            {
                _db.Categories.Update(OBJ);
                _db.SaveChanges();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _db.Categories.Find(id);
            
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? obj = _db.Categories.Find(id);
            if(obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");

            
        }
    }
}