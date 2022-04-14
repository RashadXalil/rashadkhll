using FinalBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static FinalBack.Models.PaginatedList;

namespace FinalBack.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]


    public class SliderController : Controller
    {
        private readonly FinalContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(FinalContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]
        public IActionResult Index(int page = 1,bool? isDeleted=null)
        {
            ViewBag.isDeleted = isDeleted;

            var Sliders = _context.Sliders.AsQueryable();

            if (isDeleted != null)
                Sliders = Sliders.Where(x => x.IsDeleted == isDeleted).AsQueryable();

            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<Slider>.Create(Sliders, page, pageSize));
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (!ModelState.IsValid) return View();

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
            TempData["Success"] = "Slider Created !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin,Admin,Editor")]
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
            existSlider.Title1 = slider.Title1;
            existSlider.Title2 = slider.Title2;
            existSlider.Desc = slider.Desc;
            existSlider.BtnText = slider.BtnText;
            _context.SaveChanges();
            TempData["Success"] = "Slider Edited !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            Slider slider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (slider is null) return NotFound();
            slider.IsDeleted = true;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Slider Deleted !";
            return RedirectToAction("index");
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Restore(int id)
        {
            Slider existslider = _context.Sliders.FirstOrDefault(x => x.Id == id && x.IsDeleted);

            if (existslider == null)
                return NotFound();

            existslider.IsDeleted = false;
            _context.SaveChanges();
            TempData["Success"] = "Slider Restored !";
            return RedirectToAction("index");
        }


    }
}
