using FinalBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static FinalBack.Models.PaginatedList;

namespace FinalBack.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]



    public class BrandController : Controller
    {
        private readonly FinalContext _context;
        private readonly IWebHostEnvironment _env;

        public BrandController(FinalContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]
        public IActionResult Index(int page = 1,bool? isDeleted=null)
        {
            ViewBag.isDeleted = isDeleted;
            var Brands = _context.Brands.AsQueryable();
            if (isDeleted != null)
                Brands = Brands.Where(x => x.IsDeleted == isDeleted).AsQueryable();
            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<Brand>.Create(Brands, page, pageSize));
        }
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin,Admin,Creator")]
        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            if (!ModelState.IsValid) return View();

            if (brand.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image file is required!");
                return View();
            }

            if (brand.ImageFile.ContentType != "image/jpeg" && brand.ImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("ImageFile", "file type must be image/jpeg or image/png");
                return View();
            }

            if (brand.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "file size must be less than 2mb");
                return View();
            }

            brand.Image = Guid.NewGuid().ToString() + brand.ImageFile.FileName;

            string path = Path.Combine(_env.WebRootPath, "uploads/brands", brand.Image);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                brand.ImageFile.CopyTo(stream);
            }

            _context.Brands.Add(brand);
            _context.SaveChanges();
            TempData["Success"] = "Brand Created !";
            return RedirectToAction("index");


        }
        [Authorize(Roles = "SuperAdmin,Admin,Editor")]
        public IActionResult Edit(int id)
        {
            Brand brand = _context.Brands.FirstOrDefault(x => x.Id == id);
            if (brand == null)
                return NotFound();

            return View(brand);
        }

        [HttpPost]
        public IActionResult Edit(Brand brand)
        {
            if (!ModelState.IsValid)
                return Json(brand);

            Brand existbrand = _context.Brands.FirstOrDefault(x => x.Id == brand.Id);
            if (existbrand == null) return NotFound();

            if (brand.ImageFile != null)
            {
                if (brand.ImageFile.ContentType != "image/jpeg" && brand.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "file type must be image/jpeg or image/png");
                    return View();
                }

                if (brand.ImageFile.Length > 2097152)
                {

                    ModelState.AddModelError("ImageFile", "file size must be less than 2mb");
                    return View();
                }

                brand.Image = Guid.NewGuid().ToString() + brand.ImageFile.FileName;


                string path = Path.Combine(_env.WebRootPath, "uploads/brands", brand.Image);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    brand.ImageFile.CopyTo(stream);
                }

                if (existbrand.Image != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads/brands", existbrand.Image);
                    if (System.IO.File.Exists(existPath))
                        System.IO.File.Delete(existPath);
                }

                existbrand.Image = brand.Image;

            }
            else
            {
                if (brand.Image == null && existbrand.Image != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads/brands", existbrand.Image);
                    if (System.IO.File.Exists(existPath))
                        System.IO.File.Delete(existPath);

                    existbrand.Image = null;
                }
            }
            existbrand.Name = brand.Name;
            _context.SaveChanges();
            TempData["Success"] = "Brand Edited !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            Brand brand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == id);
            if (brand is null) return NotFound();
            brand.IsDeleted = true;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Brand Deleted !";
            return RedirectToAction("index");
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Restore(int id)
        {
            Brand existbrand = _context.Brands.FirstOrDefault(x => x.Id == id && x.IsDeleted);

            if (existbrand == null)
                return NotFound();

            existbrand.IsDeleted = false;
            _context.SaveChanges();
            TempData["Success"] = "Brand Restored !";
            return RedirectToAction("index");
        }
    }
}
