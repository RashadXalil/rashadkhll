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


    public class FaqController : Controller
    {
        private readonly FinalContext _context;
        private readonly IWebHostEnvironment _env;

        public FaqController(FinalContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator,Editor")]
        public IActionResult Index(int page=1,bool? isDeleted=null)
        {
            ViewBag.isDeleted = isDeleted;

            var Faqs = _context.Faqs.AsQueryable();

            if (isDeleted != null)
                Faqs = Faqs.Where(x => x.IsDeleted == isDeleted).AsQueryable();

            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<Faq>.Create(Faqs, page, pageSize));
        }
        [Authorize(Roles = "SuperAdmin,Admin,Creator")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Faq faq)
        {
            if (!ModelState.IsValid) return View();


            _context.Faqs.Add(faq);
            _context.SaveChanges();
            TempData["Success"] = "Faq Created !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin,Admin,Editor")]
        public IActionResult Edit(int id)
        {
            Faq faq = _context.Faqs.FirstOrDefault(x => x.Id == id);

            if (faq == null) return NotFound();

            return View(faq);
        }
        [HttpPost]
        public IActionResult Edit(Faq faq)
        {
            if (!ModelState.IsValid)
                return View();

            Faq existfaq = _context.Faqs.FirstOrDefault(x => x.Id == faq.Id);
            if (existfaq == null) return NotFound();

            existfaq.FaqHeader = faq.FaqHeader;
            existfaq.FaqBody = faq.FaqBody;

            _context.SaveChanges();
            TempData["Success"] = "Faq Edited !";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            Faq faq = await _context.Faqs.FirstOrDefaultAsync(x => x.Id == id);
            if (faq is null) return NotFound();
            faq.IsDeleted = true;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Faq Deleted !";
            return RedirectToAction("index");
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Restore(int id)
        {
            Faq existfaq = _context.Faqs.FirstOrDefault(x => x.Id == id && x.IsDeleted);

            if (existfaq == null)
                return NotFound();

            existfaq.IsDeleted = false;
            _context.SaveChanges();
            TempData["Success"] = "Faq Restored !";
            return RedirectToAction("index");
        }
    }
}
