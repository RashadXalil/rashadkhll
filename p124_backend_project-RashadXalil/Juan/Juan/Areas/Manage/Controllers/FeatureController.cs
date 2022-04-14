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

    public class FeatureController : Controller
    {
        private readonly JuanContext _context;
        private readonly IWebHostEnvironment _env;

        public FeatureController(JuanContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<Features>.Create(_context.Features.AsQueryable(), page, pageSize));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Features feature)
        {
            if (feature.ImageFile == null)
                ModelState.AddModelError("ImageFile", "Image file is required!");

            if (feature.ImageFile.ContentType != "image/jpeg" && feature.ImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("ImageFile", "file type must be image/jpeg or image/png");
                return View();
            }

            if (feature.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "file size must be less than 2mb");
                return View();
            }

            feature.Image = Guid.NewGuid().ToString() + feature.ImageFile.FileName;

            string path = Path.Combine(_env.WebRootPath, "uploads/features", feature.Image);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                feature.ImageFile.CopyTo(stream);
            }

            _context.Features.Add(feature);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Edit(int id)
        {
            Features feature = _context.Features.FirstOrDefault(x => x.Id == id);

            if (feature == null) return NotFound();

            return View(feature);
        }

        [HttpPost]
        public IActionResult Edit(Features feature)
        {
            if (!ModelState.IsValid)
                return View();

            Features existFeature= _context.Features.FirstOrDefault(x => x.Id == feature.Id);
            if (existFeature == null) return NotFound();

            if (feature.ImageFile != null)
            {
                if (feature.ImageFile.ContentType != "image/jpeg" && feature.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "file type must be image/jpeg or image/png");
                    return View();
                }

                if (feature.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "file size must be less than 2mb");
                    return View();
                }

                feature.Image = Guid.NewGuid().ToString() + feature.ImageFile.FileName;


                string path = Path.Combine(_env.WebRootPath, "uploads/features", feature.Image);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    feature.ImageFile.CopyTo(stream);
                }

                if (existFeature.Image != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads/features", existFeature.Image);
                    if (System.IO.File.Exists(existPath))
                        System.IO.File.Delete(existPath);
                }

                existFeature.Image = feature.Image;
            }
            else
            {
                if (feature.Image == null && existFeature.Image != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads/features", existFeature.Image);
                    if (System.IO.File.Exists(existPath))
                        System.IO.File.Delete(existPath);

                    existFeature.Image = null;
                }
            }


            existFeature.Title = feature.Title;
            existFeature.Desc = feature.Desc;

            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Features feature = _context.Features.FirstOrDefault(x => x.Id == id);

            return View(feature);
        }

        [HttpPost]
        public IActionResult Delete(Features feature)
        {
            Features existfeature = _context.Features.FirstOrDefault(x => x.Id == feature.Id);

            if (existfeature == null)
                return NotFound();

            _context.Features.Remove(existfeature);
            _context.SaveChanges();

            return RedirectToAction("index");
        }


    }
}
