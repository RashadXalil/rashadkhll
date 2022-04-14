using FinalBack.Models;
using FinalBack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FinalBack.Controllers
{
    public class HomeController : Controller
    {
        private readonly FinalContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(FinalContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            HomeViewModel homeVM = new HomeViewModel()
            {
                Sliders = _context.Sliders.ToList(),
                Features = _context.Features.Where(x => x.IsDeleted == false).ToList(),
                Blogs = _context.Blogs.Where(x => x.IsDeleted == false).Take(3).ToList(),
                Settings = _context.Settings.ToDictionary(x => x.Key, x => x.Value),
                Brands = _context.Brands.Where(x => x.IsDeleted == false).ToList(),
                HotTrendingProducts = _context.Products.Include(x=>x.Reviews).Include(x => x.Brand).Include(x => x.SubCategory).ThenInclude(x => x.Category).Include(x => x.Color).Include(x => x.ProductImages).Where(x => x.IsTrending == true).Where(x => x.IsDeleted == false).Take(4).ToList(),
                TopDealProducts = _context.Products.Include(x=>x.Reviews).Include(x => x.Brand).Include(x => x.SubCategory).ThenInclude(x => x.Category).Include(x => x.Color).Include(x => x.ProductImages).Where(x => x.IsNew == true).Where(x => x.IsDeleted == false).Take(2).ToList(),
                PopularCategories = _context.Categories.Where(x => x.IsDeleted == false).Take(6).ToList(),
                AllProducts = _context.Products.Include(x=>x.Reviews).Where(x=>x.IsDeleted==false).Include(x=>x.SubCategory).ThenInclude(x=>x.Category).ToList(),
                RecommendingProducts = _context.Products.Include(x=>x.Reviews).Include(x => x.Brand).Include(x => x.SubCategory).ThenInclude(x => x.Category).Include(x => x.Color).Include(x => x.ProductImages).Where(x => x.IsFeatured == true).Where(x => x.IsDeleted == false).Take(4).ToList(),
                TopFeaturedProductBanner = _context.Products.Include(x=>x.Reviews).Include(x=>x.Brand).Include(x=>x.ProductImages).Include(x=>x.SubCategory).ThenInclude(x=>x.Category).Include(x=>x.Color).Where(x=>x.IsDeleted==false).Where(x=>x.IsFeatured==true).Where(x=>x.IsNew==true).Where(x=>x.IsTrending==true).Take(1).ToList(),
                TopFeaturedProducts = _context.Products.Include(x=>x.Reviews).Include(x => x.Brand).Include(x => x.ProductImages).Include(x => x.SubCategory).ThenInclude(x => x.Category).Include(x => x.Color).Where(x => x.IsDeleted == false).Where(x => x.IsFeatured == true).Take(4).ToList(),
            };
            return View(homeVM);
        }
        public IActionResult Error()
        {
            return View();
        }
        public IActionResult GetProduct(int? id = null)
        {
            var product = _context.Products.Include(x=>x.ProductImages).Include(x => x.Brand).Include(x => x.SubCategory).ThenInclude(x=>x.Category).Include(x => x.Color).FirstOrDefault(x => x.Id == id);
            return PartialView("_ProductModalPartial", product);

        }
        public IActionResult ProductDetail(int id)
        {
            if (id < 0) return NotFound();
            var product = _context.Products.Include(x => x.Reviews).ThenInclude(x => x.AppUser).Include(x => x.ProductImages).Include(x => x.Brand).Include(x => x.SubCategory).ThenInclude(x => x.Category).Include(x => x.Color).Include(x => x.ProductImages).FirstOrDefault(x => x.Id == id);
            if (product is null) return RedirectToAction("home", "error");
            HomeViewModel homeVM = new HomeViewModel()
            {
                Product=_context.Products.Include(x=>x.Reviews).ThenInclude(x=>x.AppUser).Include(x=>x.ProductImages).Include(x=>x.Brand).Include(x=>x.SubCategory).ThenInclude(x=>x.Category).Include(x=>x.Color).Include(x=>x.ProductImages).FirstOrDefault(x=>x.Id==id),
                Review = new Review { ProductId = id},
            };
            return View(homeVM);    
        }
        public IActionResult AboutUs()
        {
            HomeViewModel homeVM = new HomeViewModel()
            {
                Services = _context.Services.Where(x => x.IsDeleted == false).ToList(),
                Members = _context.TeamMembers.Where(x=>x.IsDeleted==false).ToList(),
                Branches=_context.Branches.Where(x=>x.IsDeleted==false).ToList(),
            };
            return View(homeVM);
        }
        public IActionResult FAQ()
        {
            HomeViewModel homeVM = new HomeViewModel()
            {
                LeftFaqs = _context.Faqs.Where(x=>x.IsDeleted==false).Take(3).ToList(),
                RightFaqs = _context.Faqs.Where(x=>x.IsDeleted==false).Skip(3).Take(3).ToList()
            };
            return View(homeVM);
        }
        public IActionResult Contactus()
        {
            HomeViewModel homeVM = new HomeViewModel()
            {
                Branches = _context.Branches.Where(x => x.IsDeleted == false).ToList(),
            };
            return View(homeVM);
        }
        public IActionResult TrackOrder()
        {
            return View();
        }
        [HttpPost]
        public IActionResult TrackOrder(string trackId)
        {
            Order order = _context.Orders.Include(x=>x.OrderItems).FirstOrDefault(x => x.TrackId == trackId);
            if (order == null) return NotFound();
            return RedirectToAction("GetOrderByTrackId",order);
        }
        public IActionResult GetOrderByTrackId(Order order)
        {
            return View(order);
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
             .Include(x => x.Brand).Include(x => x.SubCategory).ThenInclude(x=>x.Category)
             .Include(x => x.Color)
             .Include(x => x.Reviews).ThenInclude(x => x.AppUser)
             .FirstOrDefault(x => x.Id == review.ProductId);

            if (product == null)
                return RedirectToAction("error","home");
            List<Review> userreviews = product.Reviews.Where(x => x.AppUser.Email == member.Email).ToList();
            if (userreviews.Count() >= 1)
            {
                ModelState.AddModelError("Text", "You Can add only 1 review");
                HomeViewModel HomeVM = new HomeViewModel
                {
                    Product = product,
                    Review = review,
                    AllProducts = _context.Products.Include(x => x.Reviews).Include(x => x.Brand).Include(x => x.ProductImages).Include(x => x.SubCategory).ThenInclude(x => x.Category).Include(x => x.Color).Where(x => x.BrandId == product.Brand.Id).Take(6).ToList()
                };

                return View("ProductDetail", HomeVM);
            }
            if (!ModelState.IsValid)
            {


                HomeViewModel HomeVM = new HomeViewModel
                {
                    Product = product,
                    Review = review,
                    AllProducts = _context.Products.Include(x=>x.Reviews).Include(x=>x.Brand).Include(x=>x.ProductImages).Include(x=>x.SubCategory).ThenInclude(x=>x.Category).Include(x=>x.Color).Where(x => x.BrandId == product.Brand.Id).Take(6).ToList()
                };

                return View("ProductDetail", HomeVM);
            }
            review.AppUserId = member.Id;
            review.AppUser = member;
            review.CreatedAt = DateTime.UtcNow.AddHours(4);
            product.Reviews.Add(review);
            product.Rate = (byte)Math.Ceiling(product.Reviews.Sum(x => x.Rate) / (double)product.Reviews.Count);           
            _context.SaveChanges();

            TempData["Success"] = "Review Added !";

            return RedirectToAction("ProductDetail", new { id = review.ProductId });
        }
    }
}
