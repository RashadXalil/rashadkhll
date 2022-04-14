using Juan.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Juan.Models.PaginatedList;

namespace Juan.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin")]

    public class GenderController : Controller
    {
        private readonly JuanContext _context;
        private readonly IWebHostEnvironment _env;

        public GenderController(JuanContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<Gender>.Create(_context.Genders.AsQueryable(), page, pageSize));
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Gender gender)
        {


            _context.Genders.Add(gender);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Edit(int id)
        {
            Gender gender = _context.Genders.FirstOrDefault(x => x.Id == id);

            if (gender == null) return NotFound();

            return View(gender);
        }

        [HttpPost]
        public IActionResult Edit(Gender gender)
        {
            if (!ModelState.IsValid)
                return View();

            Gender existGender= _context.Genders.FirstOrDefault(x => x.Id == gender.Id);
            if (existGender == null) return NotFound();

            existGender.Name = gender.Name;

            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Gender gender = _context.Genders.FirstOrDefault(x => x.Id == id);

            return View(gender);
        }

        [HttpPost]
        public IActionResult Delete(Gender gender)
        {
            Gender existGender = _context.Genders.FirstOrDefault(x => x.Id == gender.Id);

            if (existGender == null)
                return NotFound();

            _context.Genders.Remove(existGender);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

    }
}
