
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
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
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitofwork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofwork = unitofwork;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            List<Product> productList = _unitofwork.product.GetAll(includeProperties:"Category").ToList();
            return View(productList);
        }
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryList = _unitofwork.category.GetAll().Select(u => new SelectListItem
            //{
            //    Text = u.Name,
            //    Value = u.Id.ToString()
            //});
            //// ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;
            ProductVM productVM = new ()
            {
                CategoryList = _unitofwork.category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitofwork.product.Get(u=>u.Id==id,includeProperties:"ProductImages");
                return View(productVM);

            }
           
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM,List<IFormFile> files)
        {
            //if (OBJ.Title == OBJ.Title.ToString())
            //{
            //    ModelState.AddModelError("name", "Title CANNOT MATCH THE NAME");
            //}
            

            if (ModelState.IsValid)
            {
                if (productVM.Product.Id == 0)
                {
                    _unitofwork.product.Add(productVM.Product);

                }
                else
                {
                    _unitofwork.product.update(productVM.Product);
                }
                _unitofwork.save();
                string wwwroot = _webHostEnvironment.WebRootPath;
                if (files != null)
                {
                    foreach (IFormFile file in files)
                    {

                        //filename is random guid
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        //location where to save file
                        string productPath = @"images\products\product-" + productVM.Product.Id;
                        string finalPath = Path.Combine(wwwroot, productPath);

                        if (!Directory.Exists(finalPath))
                        {
                            Directory.CreateDirectory(finalPath);
                        }
                        using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        ProductImage productImage = new()
                        {
                            ImageUrl = @"\" + productPath + @"\" + fileName,
                            ProductId=productVM.Product.Id,
                        };
                        if(productVM.Product.ProductImages==null)
                            productVM.Product.ProductImages = new List<ProductImage>();
                        productVM.Product.ProductImages.Add(productImage);
                       
                    }
                    _unitofwork.product.update(productVM.Product);
                    _unitofwork.save();

                   
                }
                
                
                TempData["success"] = "Product Created/Updated Successfully";
                return RedirectToAction("Index");
            }
            else
            {

                productVM.CategoryList = _unitofwork.category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                    
                
                return View(productVM);
            }

           
        }
        public IActionResult DeleteImage(int imageId)
        {
            var imageToBeDeleted = _unitofwork.productImage.Get(u => u.Id == imageId);
            int productId = imageToBeDeleted.ProductId;
            if (imageToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {
                    var oldImagePath =
                                   Path.Combine(_webHostEnvironment.WebRootPath,
                                   imageToBeDeleted.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitofwork.productImage.Remove(imageToBeDeleted);
                _unitofwork.save();

                TempData["success"] = "Deleted successfully";
            }

            return RedirectToAction(nameof(Upsert), new { id = productId });
        }

        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    // Category? categoryFromDb = _categoryRepo.Categories.Find(id);
        //    Product? productFromDb = _unitofwork.product.Get(u => u.Id == id);
        //    //Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();
        //    if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(productFromDb);
        //}
        //[HttpPost]
        //public IActionResult Edit(Product OBJ)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        _unitofwork.product.update(OBJ);
        //        _unitofwork.save();
        //        TempData["success"] = "Product Updated Successfully";
        //        return RedirectToAction("Index");
        //    }

        //    return View();
        //}

        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? productFromDb = _unitofwork.product.Get(u => u.Id == id);

        //    if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(productFromDb);
        //}
        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePost(int? id)
        //{
        //    Product? obj = _unitofwork.product.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitofwork.product.Remove(obj);
        //    _unitofwork.save();
        //    TempData["success"] = "Product Deleted Successfully";
        //    return RedirectToAction("Index");


        // }
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> productList = _unitofwork.product.GetAll(includeProperties: "Category").ToList();
            return Json(new {data=productList});
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
      {
            var productToBeDelete = _unitofwork.product.Get(u => u.Id == id);
            if(productToBeDelete == null) 
            { 
                return Json(new {success=false, message="Error while deleting"});
            }
            string productPath = @"images\products\product-" + id;
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);

            if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }

                Directory.Delete(finalPath);
            }

            _unitofwork.product.Remove(productToBeDelete);
            _unitofwork.save();
           
            return Json(new { success=true,message="Delete successful" });
        }

        #endregion
    }
}
