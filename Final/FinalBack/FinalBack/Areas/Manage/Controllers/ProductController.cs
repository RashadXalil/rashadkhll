using FinalBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FinalBack.Models.PaginatedList;

namespace FinalBack.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]

    public class ProductController : Controller
    {
        private readonly FinalContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(FinalContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]
        public IActionResult Index(int page = 1, bool? isDeleted = null)
        {
            ViewBag.IsDeleted = isDeleted;

            var Products = _context.Products.Include(x => x.Brand).Include(x => x.SubCategory).ThenInclude(x=>x.Category).Include(x => x.ProductImages).AsQueryable();

            if (isDeleted != null)
                Products = Products.Where(x => x.IsDeleted == isDeleted);

            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = pageSizeStr == null ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<Product>.Create(Products, page, pageSize));
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator")]
        public IActionResult Create()
        {
            ViewBag.SubCategories = _context.SubCategories.Where(x => !x.IsDeleted).ToList();
            ViewBag.Brands = _context.Brands.Where(x => !x.IsDeleted).ToList();
            ViewBag.Colors = _context.Colors.Where(x => !x.IsDeleted).ToList();
            return View();
        }


        [HttpPost]
        public IActionResult Create(Product product)
        {
            ViewBag.SubCategories = _context.SubCategories.Where(x => !x.IsDeleted).ToList();
            ViewBag.Brands = _context.Brands.Where(x => !x.IsDeleted).ToList();
            ViewBag.Colors = _context.Colors.Where(x => !x.IsDeleted).ToList();
            ViewBag.Subcategories = _context.SubCategories.ToList();

            if (!_context.Brands.Any(x => x.Id == product.BrandId && !x.IsDeleted))
            {
                return RedirectToAction("error", "home");
            }
            if (!_context.Categories.Any(x => x.Id == product.SubCategoryId && !x.IsDeleted))
            {
                return RedirectToAction("error", "home");

            }
            if (!_context.Colors.Any(x => x.Id == product.ColorId && !x.IsDeleted))
            {
                return RedirectToAction("error", "home");
            }

            if (!ModelState.IsValid)
                return View();

            if (product.ThumbFile == null)
            {
                ModelState.AddModelError("ThumbFile", "PosterFile is required");
                return View();
            }
            else
            {
                if (product.ThumbFile.ContentType != "image/jpeg" && product.ThumbFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ThumbFile", "file type must be image/jpeg or image/png");
                    return View();
                }

                if (product.ThumbFile.Length > 2097152)
                {
                    ModelState.AddModelError("ThumbFile", "file size must be less than 2mb");
                    return View();
                }
            }

            if (product.Files != null)
            {
                foreach (var file in product.Files)
                {
                    if (file.ContentType != "image/jpeg" && file.ContentType != "image/png")
                    {
                        ModelState.AddModelError("Files", "file type must be image/jpeg or image/png");
                        return View();
                    }

                    if (file.Length > 2097152)
                    {
                        ModelState.AddModelError("Files", "file size must be less than 2mb");
                        return View();
                    }
                }
                if (product.Files.Count > 2)
                {
                    ModelState.AddModelError("Files", "You Can Choose Max 2 Image");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("Files", "File is required");
                return View();
            }


            if (!_context.Brands.Any(x => x.Id == product.BrandId && !x.IsDeleted))
            {
                ModelState.AddModelError("BrandId", "Brand not found");
                return View();
            }
            if (!_context.SubCategories.Any(x => x.Id == product.SubCategoryId && !x.IsDeleted))
            {
                ModelState.AddModelError("CategoryId", "Category not found");
                return View();
            }

            product.ProductImages = new List<ProductImage>();

            ProductImage ThumbImage = new ProductImage
            {
                ThumbStatus = true,
                Image = FileManager.Save(_env.WebRootPath, "uploads/products", product.ThumbFile)
            };
            product.ProductImages.Add(ThumbImage);

            if (product.Files != null)
            {
                foreach (var file in product.Files)
                {
                    ProductImage productImage = new ProductImage
                    {
                        ThumbStatus = null,
                        Image = FileManager.Save(_env.WebRootPath, "uploads/products", file)
                    };
                    product.ProductImages.Add(productImage);
                }
            }

            _context.Products.Add(product);
            _context.SaveChanges();
            TempData["Success"] = "Product Created !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin,Admin,Editor")]
        public IActionResult Edit(int id)
        {
            Product product = _context.Products.Include(x => x.ProductImages).Include(x => x.SubCategory).ThenInclude(x=>x.Category).Include(x=>x.Brand).FirstOrDefault(x => x.Id == id);
            if (product == null) return NotFound();

            ViewBag.SubCategories = _context.SubCategories.Where(x => !x.IsDeleted).ToList();
            ViewBag.Brands = _context.Brands.Where(x => !x.IsDeleted).ToList();
            ViewBag.Colors = _context.Colors.Where(x => !x.IsDeleted).ToList();

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            ViewBag.SubCategories = _context.SubCategories.Where(x => !x.IsDeleted).ToList();
            ViewBag.Brands = _context.Brands.Where(x => !x.IsDeleted).ToList();
            ViewBag.Colors = _context.Colors.Where(x => !x.IsDeleted).ToList();

            Product existProduct = _context.Products.Include(c => c.ProductImages).Include(x=>x.SubCategory).ThenInclude(x=>x.Category).Include(x=>x.Brand).FirstOrDefault(x => x.Id == product.Id);

            if (existProduct == null) return NotFound();

            if (!_context.Brands.Any(x => x.Id == product.BrandId && !x.IsDeleted))
            {
                return RedirectToAction("error", "home");
            }
            if (!_context.SubCategories.Any(x => x.Id == product.SubCategoryId && !x.IsDeleted))
            {
                return RedirectToAction("error", "home");

            }
            if (!_context.Colors.Any(x => x.Id == product.ColorId && !x.IsDeleted))
            {
                return RedirectToAction("error", "home");

            }

            if (product.ThumbFile != null && product.ThumbFile.ContentType != "image/jpeg" && product.ThumbFile.ContentType != "image/png")
            {
                return RedirectToAction("error", "home");

            }

            if (product.ThumbFile != null && product.ThumbFile.Length > 2097152)
            {
                ModelState.AddModelError("ThumbFile", "file size must be less than 2mb");
                return View();
            }

            if (product.Files != null)
            {
                foreach (var file in product.Files)
                {
                    if (file.ContentType != "image/jpeg" && file.ContentType != "image/png")
                    {
                        ModelState.AddModelError("Files", "file type must be image/jpeg or image/png");
                        return View();
                    }

                    if (file.Length > 2097152)
                    {
                        ModelState.AddModelError("Files", "file size must be less than 2mb");
                        return View();
                    }
                }
            }

            ProductImage thumb = existProduct.ProductImages.FirstOrDefault(x => x.ThumbStatus == true);


            if (product.ThumbFile != null)
            {
                string newPosterImg = FileManager.Save(_env.WebRootPath, "uploads/products", product.ThumbFile);
                if (thumb != null)
                {
                    FileManager.Delete(_env.WebRootPath, "uploads/products", thumb.Image);
                    thumb.Image = newPosterImg;
                }
                else
                {
                    thumb = new ProductImage { Image = newPosterImg, ThumbStatus = true };
                    existProduct.ProductImages.Add(thumb);
                }
            }

            existProduct.ProductImages.RemoveAll(x => x.ThumbStatus == null && !product.FileIds.Contains(x.Id));

            if (product.Files != null)
            {
                foreach (var file in product.Files)
                {
                    ProductImage productImage = new ProductImage
                    {
                        ThumbStatus = null,
                        Image = FileManager.Save(_env.WebRootPath, "uploads/products", file)
                    };
                    existProduct.ProductImages.Add(productImage);
                }
            }


            existProduct.BrandId = product.BrandId;
            existProduct.SubCategoryId = product.SubCategoryId;
            existProduct.CostPrice = product.CostPrice;
            existProduct.SalePrice = product.SalePrice;
            existProduct.Rate = product.Rate;
            existProduct.Name = product.Name;
            existProduct.IsNew = product.IsNew;
            existProduct.IsFeatured = product.IsFeatured;
            existProduct.IsTrending = product.IsTrending;
            existProduct.Ram = product.Ram;
            existProduct.Weight = product.Weight;
            existProduct.OperationSystem = product.OperationSystem;
            existProduct.SoundSystem = product.SoundSystem;
            existProduct.ScreenSize = product.ScreenSize;
            existProduct.Desc = product.Desc;
            existProduct.ModifiedAt = DateTime.UtcNow.AddHours(4);

            _context.SaveChanges();
            TempData["Success"] = "Product Edited !";

            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            Product product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product is null) return NotFound();
            product.IsDeleted = true;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Product Deleted !";
            return RedirectToAction("index");
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Restore(int id)
        {
            Product existproduct = _context.Products.FirstOrDefault(x => x.Id == id && x.IsDeleted);

            if (existproduct == null)
                return NotFound();

            existproduct.IsDeleted = false;
            _context.SaveChanges();
            TempData["Success"] = "Product Restored !";
            return RedirectToAction("index");
        }
    }
}
