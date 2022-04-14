using Juan.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Controllers
{
    public class ContactusController : Controller
    {
        private readonly JuanContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ContactusController(JuanContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index(ContactUs contact)
        {
            if (!ModelState.IsValid)
                return View();

            _context.ContactUses.Add(contact);
            TempData["Success"] = "Email Sent !";
            _context.SaveChanges();
            return RedirectToAction("index","home");
        }
    }
}
