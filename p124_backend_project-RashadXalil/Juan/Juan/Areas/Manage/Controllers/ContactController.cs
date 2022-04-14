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
    public class ContactController : Controller
    {
        private readonly JuanContext _context;
        private readonly IWebHostEnvironment _env;

        public ContactController(JuanContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page=1)
        {
            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = string.IsNullOrWhiteSpace(pageSizeStr) ? 3 : int.Parse(pageSizeStr);
            return View(PagenatedList<ContactUs>.Create(_context.ContactUses.AsQueryable(), page, pageSize));
        }
        public IActionResult Info(int id)
        {
            ContactUs contact = _context.ContactUses.FirstOrDefault(x => x.Id == id);

            return View(contact);
        }
        [HttpPost]
        public IActionResult Info(ContactUs contact)
        {
            ContactUs existcontact = _context.ContactUses.FirstOrDefault(x => x.Id == contact.Id);
            if (existcontact == null)
                return NotFound();
            return View();
        }
        public IActionResult Delete(int id)
        {
            ContactUs contact = _context.ContactUses.FirstOrDefault(x => x.Id == id);

            return View(contact);
        }
        [HttpPost]
        public IActionResult Delete(ContactUs contact)
        {
            ContactUs existcontact = _context.ContactUses.FirstOrDefault(x => x.Id == contact.Id);

            if (existcontact == null)
                return NotFound();

            _context.ContactUses.Remove(existcontact);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
