using Juan.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Juan.Models.PaginatedList;

namespace Juan.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin")]

    public class ProductController : Controller
    {
        private readonly JuanContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(JuanContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<Product>.Create(_context.Products.Include(x=>x.Brand).Include(x=>x.Category).Include(x=>x.Gender).Include(x=>x.productColors).AsQueryable(), page, pageSize));
        }
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Genders = _context.Genders.ToList();
            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Colors = _context.Colors.ToList();

            return View();
        }


        [HttpPost]
        public IActionResult Create(Product product)
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Genders = _context.Genders.ToList();
            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Colors = _context.Colors.ToList();

            product.productColors = new List<ProductColor>();

            if (product.ColorIds != null)
            {
                foreach (var colorId in product.ColorIds)
                {
                    if (_context.Colors.Any(x => x.Id == colorId))
                    {
                        ProductColor productColor = new ProductColor
                        {
                            ColorId = colorId,
                        };
                        product.productColors.Add(productColor);
                    }
                    else
                    {
                        ModelState.AddModelError("ColorIds", "Color not found");
                        return View();
                    }
                }
            }
            if (product.ImageFile == null)
                ModelState.AddModelError("ImageFile", "Image file is required!");

            if (product.ImageFile.ContentType != "image/jpeg" && product.ImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("ImageFile", "file type must be image/jpeg or image/png");
                return View();
            }

            if (product.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "file size must be less than 2mb");
                return View();
            }

            product.Image = Guid.NewGuid().ToString() + product.ImageFile.FileName;

            string path = Path.Combine(_env.WebRootPath, "uploads/products", product.Image);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                product.ImageFile.CopyTo(stream);
            }
            
            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("index");

           
        }

        public IActionResult Edit(int id)
        {
            Product product = _context.Products.Include(x=>x.productColors).ThenInclude(x=>x.Color).Include(x=>x.Category).Include(x=>x.Gender).Include(x=>x.Brand).FirstOrDefault(x => x.Id == id);
            if (product == null)
                return NotFound();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Genders = _context.Genders.ToList();
            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Colors = _context.Colors.ToList();
            product.ColorIds = product.productColors.Select(x => x.ColorId).ToList();
                
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {          
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Genders = _context.Genders.ToList();
            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Colors = _context.Colors.ToList();
            if (!ModelState.IsValid)
                    return Json(product);

            //return View();

            Product existproduct = _context.Products.Include(x=>x.Brand).Include(x=>x.Category).Include(x=>x.Gender).Include(x=>x.productColors).FirstOrDefault(x => x.Id == product.Id);
            if (existproduct == null) return NotFound();

            if (product.ImageFile != null)
            {
                if (product.ImageFile.ContentType != "image/jpeg" && product.ImageFile.ContentType != "image/png")
                {
                    return Json("Image type");
                    ModelState.AddModelError("ImageFile", "file type must be image/jpeg or image/png");
                    return View();
                }

                if (product.ImageFile.Length > 2097152)
                {
                    return Json("Image size");

                    ModelState.AddModelError("ImageFile", "file size must be less than 2mb");
                    return View();
                }

                product.Image = Guid.NewGuid().ToString() + product.ImageFile.FileName;


                string path = Path.Combine(_env.WebRootPath, "uploads/products", product.Image);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    product.ImageFile.CopyTo(stream);
                }

                if (existproduct.Image != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads/products", existproduct.Image);
                    if (System.IO.File.Exists(existPath))
                        System.IO.File.Delete(existPath);
                }

                existproduct.Image = product.Image;
                
            }
            else
            {
                if (product.Image == null && existproduct.Image != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads/products", existproduct.Image);
                    if (System.IO.File.Exists(existPath))
                        System.IO.File.Delete(existPath);

                    existproduct.Image = null;
                }
            }
            existproduct.Name = product.Name;
            existproduct.BrandId = product.BrandId;
            existproduct.CategoryId = product.CategoryId;
            existproduct.CostPrice = product.CostPrice;
            existproduct.SalePrice = product.SalePrice;
            existproduct.DiscountPercent = product.DiscountPercent;
            existproduct.Desc = product.Desc;
            existproduct.GenderId = product.GenderId;
            existproduct.IsAvailable = product.IsAvailable;
            product.productColors = new List<ProductColor>();

            if (product.ColorIds != null)
            {
                foreach (var colorId in product.ColorIds)
                {
                    if (_context.Colors.Any(x => x.Id == colorId))
                    {
                        ProductColor productColor = new ProductColor
                        {
                            ColorId = colorId,
                        };
                        product.productColors.Add(productColor);
                    }
                    else
                    {
                        return Json(colorId);
                        ModelState.AddModelError("ColorIds", "Color not found");
                        return View();
                    }
                }
            }
            existproduct.productColors = product.productColors;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Product product = _context.Products.FirstOrDefault(x => x.Id == id);

            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {
            Product existproduct = _context.Products.FirstOrDefault(x => x.Id == product.Id);

            if (existproduct == null)
                return NotFound();

            _context.Products.Remove(existproduct);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
