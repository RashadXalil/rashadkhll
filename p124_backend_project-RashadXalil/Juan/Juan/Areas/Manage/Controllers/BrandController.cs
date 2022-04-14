using Juan.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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

    public class BrandController : Controller
    {
        private readonly JuanContext _context;
        private readonly IWebHostEnvironment _env;

        public BrandController(JuanContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<Brand>.Create(_context.Brands.AsQueryable(), page, pageSize));
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            if (brand.ImageFile == null)
                ModelState.AddModelError("ImageFile", "Image file is required!");

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

            return RedirectToAction("index");
        }
        public IActionResult Edit(int id)
        {
            Brand brand = _context.Brands.FirstOrDefault(x => x.Id == id);

            if (brand == null) return NotFound();

            return View(brand);
        }

        [HttpPost]
        public IActionResult Edit(Brand brand)
        {
            if (!ModelState.IsValid)
                return View();

            Brand existBrand = _context.Brands.FirstOrDefault(x => x.Id == brand.Id);
            if (existBrand == null) return NotFound();

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

                if (existBrand.Image != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads/brands", existBrand.Image);
                    if (System.IO.File.Exists(existPath))
                        System.IO.File.Delete(existPath);
                }

                existBrand.Image = brand.Image;
            }
            else
            {
                if (brand.Image == null && existBrand.Image != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads/brands", existBrand.Image);
                    if (System.IO.File.Exists(existPath))
                        System.IO.File.Delete(existPath);

                    existBrand.Image = null;
                }
            }


            existBrand.Name = brand.Name;
            existBrand.Image = brand.Image;

            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Brand brand = _context.Brands.FirstOrDefault(x => x.Id == id);

            return View(brand);
        }

        [HttpPost]
        public IActionResult Delete(Brand brand)
        {
            Brand existBrand = _context.Brands.FirstOrDefault(x => x.Id == brand.Id);

            if (existBrand == null)
                return NotFound();

            _context.Brands.Remove(existBrand);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

    }
}
