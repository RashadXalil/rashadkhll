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

    public class SliderController : Controller
    {
        private readonly JuanContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(JuanContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<Slider>.Create(_context.Sliders.AsQueryable(), page, pageSize));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (slider.ImageFile == null)
                ModelState.AddModelError("ImageFile", "Image file is required!");


            if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("ImageFile", "file type must be image/jpeg or image/png");
                return View();
            }

            if (slider.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "file size must be less than 2mb");
                return View();
            }

            slider.Image = Guid.NewGuid().ToString() + slider.ImageFile.FileName;

            string path = Path.Combine(_env.WebRootPath, "uploads/sliders", slider.Image);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                slider.ImageFile.CopyTo(stream);
            }


            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Edit(int id)
        {
            Slider slider = _context.Sliders.FirstOrDefault(x => x.Id == id);

            if (slider == null) return NotFound();

            return View(slider);
        }

        [HttpPost]
        public IActionResult Edit(Slider slider)
        {
            if (!ModelState.IsValid)
                return View();

            Slider existSlider = _context.Sliders.FirstOrDefault(x => x.Id == slider.Id);
            if (existSlider == null) return NotFound();

            if (slider.ImageFile != null)
            {
                if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "file type must be image/jpeg or image/png");
                    return View();
                }

                if (slider.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "file size must be less than 2mb");
                    return View();
                }

                slider.Image = Guid.NewGuid().ToString() + slider.ImageFile.FileName;


                string path = Path.Combine(_env.WebRootPath, "uploads/sliders", slider.Image);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    slider.ImageFile.CopyTo(stream);
                }

                if (existSlider.Image != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads/sliders", existSlider.Image);
                    if (System.IO.File.Exists(existPath))
                        System.IO.File.Delete(existPath);
                }

                existSlider.Image = slider.Image;
            }
            else
            {
                if (slider.Image == null && existSlider.Image != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads/sliders", existSlider.Image);
                    if (System.IO.File.Exists(existPath))
                        System.IO.File.Delete(existPath);

                    existSlider.Image = null;
                }
            }


            existSlider.Title = slider.Title;
            existSlider.Desc = slider.Desc;
            existSlider.BtnText = slider.BtnText;
            existSlider.SubTitle = slider.SubTitle;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Slider slider = _context.Sliders.FirstOrDefault(x => x.Id == id);

            return View(slider);
        }

        [HttpPost]
        public IActionResult Delete(Slider slider)
        {
            Slider existSlider = _context.Sliders.FirstOrDefault(x => x.Id == slider.Id);

            if (existSlider == null)
                return NotFound();

            _context.Sliders.Remove(existSlider);
            _context.SaveChanges();

            return RedirectToAction("index");
        }


    }
}
