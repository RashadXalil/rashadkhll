using Juan.Models;
using Juan.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Controllers
{
    public class HomeController : Controller
    {
        private readonly JuanContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(JuanContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index(int? selectId)
        {

            HomeViewModel homeVM = new HomeViewModel
            {
                Sliders = _context.Sliders.ToList(),
                Features = _context.Features.ToList(),
                Categories = _context.Categories.ToList(),
                Brands = _context.Brands.ToList(),
                Genders = _context.Genders.ToList(),
                Colors = _context.Colors.ToList(),
                Products = _context.Products.Take(20).ToList(),
                Settings = _context.Settings.ToDictionary(x => x.Key, x => x.Value),
                RatedProducts = _context.Products.Include(x=>x.Brand).Include(x=>x.Category).Include(x=>x.Gender).Include(x=>x.productColors).ThenInclude(x=>x.Color).OrderByDescending(x=>x.Rate).Take(6).ToList()
            };
            return View(homeVM);
        }
        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(ContactUs contact)
        {
            if (!ModelState.IsValid)
                return View();
            _context.ContactUses.Add(contact);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult ProductDetail(int id, int brandId)
        {
            HomeViewModel HomeVM = new HomeViewModel
            {
                Product = _context.Products.Include(x => x.Brand).Include(x => x.Category).Include(x => x.Gender).Include(x => x.productColors).ThenInclude(x => x.Color.Colors).Include(x => x.Reviews).ThenInclude(x => x.AppUser).FirstOrDefault(x => x.Id == id),
                Products = _context.Products.Include(x => x.Brand).Include(x => x.Category).Include(x => x.Gender).Include(x => x.productColors).ThenInclude(x => x.Color.Colors).Where(x => x.BrandId == brandId).ToList(),
                Review = new Review { ProductId = id },
            };
            return View(HomeVM);
        }
        public IActionResult GetProduct(int? id = null)
        {
            var product = _context.Products.Include(x => x.Brand).Include(x => x.Category).Include(x => x.Gender).Include(x => x.productColors).ThenInclude(x => x.Color).FirstOrDefault(x => x.Id == id);
            return PartialView("_ProductModalPartial", product);

        }

        public IActionResult CustomError()
        {
            return PartialView("_CustomErrorPartial");
        }
        [HttpPost]
        public IActionResult Review(Review review)
        {
            AppUser member = null;
            if (User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);
            }

            if (member == null)
                return RedirectToAction("memberlogin", "account");

            Product product = _context.Products
             .Include(x => x.Brand).Include(x => x.Category).Include(x => x.Gender)
             .Include(x => x.productColors).ThenInclude(x => x.Color)
             .Include(x => x.Reviews).ThenInclude(x => x.AppUser)
             .FirstOrDefault(x => x.Id == review.ProductId);

            if (product == null)
                return RedirectToAction("customerror");


            if (!ModelState.IsValid)
            {


                HomeViewModel bookVM = new HomeViewModel
                {
                    Product = product,
                    Review = review,
                    Products = _context.Products.Where(x => x.BrandId == product.Brand.Id).Take(6).ToList()
                };

                return View("Detail", bookVM);
            }
            review.AppUserId = member.Id;
            review.CreatedAt = DateTime.UtcNow.AddHours(4);
            product.Reviews.Add(review);
            product.Rate = (byte)Math.Ceiling(product.Reviews.Sum(x => x.Rate) / (double)product.Reviews.Count);
            _context.SaveChanges();

            TempData["Success"] = "Review Added !";

            return RedirectToAction("ProductDetail", new { id = review.ProductId });
        }

        public IActionResult Checkout()
        {

            return View();
        }
    }
    
}
