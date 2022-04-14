using FinalBack.Models;
using FinalBack.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalBack.Controllers
{
    public class WishlistController : Controller
    {
        private readonly FinalContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;

        public WishlistController(FinalContext context, IWebHostEnvironment env, UserManager<AppUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            AppUser member = null;

            if (User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);
            }

            if(member == null)
            {
                return RedirectToAction("register", "account");
            }
            else
            {
                List<WishlistItem> items = _context.WishlistItems.Include(x=>x.Product).ThenInclude(x=>x.ProductImages).Where(x => x.AppUserId == member.Id).ToList();
                return View(items);
            }
        }
        public IActionResult AddWishlist(int id)
        {
            if (!_context.Products.Any(x => x.Id == id))
            {
                return NotFound();
            }

            AppUser member = null;

            if (User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);
            }

            if (member == null)
            {
                return RedirectToAction("register", "account");
            }
            else
            {
                WishlistItem wishlistItem = _context.WishlistItems.FirstOrDefault(x => x.AppUserId == member.Id && x.ProductId == id);

                if (wishlistItem == null)
                {
                    wishlistItem = new WishlistItem
                    {
                        AppUserId = member.Id,
                        ProductId = id,
                        CreatedAt = DateTime.UtcNow.AddHours(4),
                        Count = 1
                    };
                    _context.WishlistItems.Add(wishlistItem);
                }
                _context.SaveChanges();
                List<WishlistItem> items = _context.WishlistItems.Include(x => x.Product).ThenInclude(x => x.ProductImages).ToList();
                return PartialView("_WishlistItemPartial", items);
            }
        }
        private WishlistViewModel _getWishlist(List<WishlistItemViewModel> WishlistItems)
        {
            WishlistViewModel wishlistVM = new WishlistViewModel
            {
                WishlistProducts = new List<ProductWishlistItemViewModel>(),
                TotalPrice = 0
            };

            foreach (var item in WishlistItems)
            {
                Product product = _context.Products.Include(x => x.Brand).Include(x => x.ProductImages).Include(x => x.SubCategory).Include(x => x.SubCategory).Include(x => x.Color).FirstOrDefault(x => x.Id == item.ProductId);
                ProductWishlistItemViewModel productWishlistItem = new ProductWishlistItemViewModel
                {
                    Product = product,
                    Count = item.Count
                };

                wishlistVM.WishlistProducts.Add(productWishlistItem);
                decimal totalPrice = product.DiscountPercent > 0 ? (product.SalePrice * (1 - product.DiscountPercent / 100)) : product.SalePrice;
                wishlistVM.TotalPrice += totalPrice * item.Count;
            }
            return wishlistVM;
        }
        private WishlistViewModel _getWishlist(List<WishlistItem> wishlistItems)
        {
            WishlistViewModel WishlistVM = new WishlistViewModel
            {
                WishlistProducts = new List<ProductWishlistItemViewModel>(),
                TotalPrice = 0
            };

            foreach (var item in wishlistItems)
            {
                Product product = _context.Products.Include(x => x.Brand).Include(x => x.ProductImages).Include(x => x.SubCategory).ThenInclude(x => x.Category).FirstOrDefault(x => x.Id == item.ProductId);
                ProductWishlistItemViewModel productWishlistItem = new ProductWishlistItemViewModel
                {
                    Product = product,
                    Count = item.Count
                };

                WishlistVM.WishlistProducts.Add(productWishlistItem);
                decimal totalPrice = product.DiscountPercent > 0 ? (product.SalePrice * (1 - product.DiscountPercent / 100)) : product.SalePrice;
                WishlistVM.TotalPrice += totalPrice * item.Count;
            }

            return WishlistVM;
        }
        public IActionResult GetCookie(string key)
        {
            var value = HttpContext.Request.Cookies[key];
            return Content(value);
        }
        public IActionResult RemoveWishlist(int id)
        {
                if (!_context.Products.Any(x => x.Id == id))
                {
                    return NotFound();
                }
                WishlistItem item = _context.WishlistItems.FirstOrDefault(x => x.ProductId == id);

                if (item!=null) _context.WishlistItems.Remove(item);
                _context.SaveChanges();

              
                List<WishlistItem> items = _context.WishlistItems.Include(x => x.Product).ThenInclude(x => x.ProductImages).ToList();
                TempData["Success"] = "Product removed from Wishlist !";

            return RedirectToAction("index","wishlist");
        }
    }
}
