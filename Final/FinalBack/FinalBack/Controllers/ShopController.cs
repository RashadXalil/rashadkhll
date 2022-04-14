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

namespace FinalBack.Controllers
{
    public class ShopController : Controller
    {
        private readonly FinalContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;

        public ShopController(FinalContext context, IWebHostEnvironment env, UserManager<AppUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }
        public IActionResult Index(int? subcategoryId,int? categoryId ,int? brandId, int?colorId,int? rateId, decimal? minPrice, decimal? maxPrice,int page = 1)
        {
            ViewBag.PageIndex = page;
            ViewBag.SubCategoryId = subcategoryId;
            ViewBag.CategoryId = categoryId;
            ViewBag.brandId = brandId;
            ViewBag.ColorId = colorId;
            ViewBag.RateId = rateId;

            ShopViewModel shopVM = new ShopViewModel();
            var Products = _context.Products.Include(x => x.ProductImages).Include(x => x.SubCategory).ThenInclude(x => x.Category).Include(x => x.Brand).Include(x => x.Color).Where(x=>x.IsDeleted==false).AsQueryable();
            if (subcategoryId != null)
                Products = Products.Where(x => x.SubCategory.Id == subcategoryId).AsQueryable();
            if (brandId != null)
            {
                Products = Products.Where(x => x.BrandId == brandId);
            }
            if (categoryId != null)
            {
                Products = Products.Where(x => x.SubCategory.CategoryId == categoryId);
            }
            if (colorId != null)
                Products = Products.Where(x => x.Color.Id == colorId).AsQueryable();

            if (Products.Any())
            {
                shopVM.MaxPrice = Products.Max(x => x.SalePrice);
                shopVM.MinPrice = Products.Min(x => x.SalePrice);
            }
            ViewBag.FilterMinPrice = minPrice ?? shopVM.MinPrice;
            ViewBag.FilterMaxPrice = maxPrice ?? shopVM.MaxPrice;

            if (minPrice != null && maxPrice != null)
            {
                Products = Products.Where(x => x.DiscountPercent > 0 ? x.SalePrice * (1 - x.DiscountPercent / 100) >= minPrice && x.SalePrice * (1 - x.DiscountPercent / 100) <= maxPrice : x.SalePrice >= minPrice && x.SalePrice <= maxPrice);
            }
            ViewBag.TotalPage = (int)Math.Ceiling(Products.Count() / 8d);
            shopVM.Colors = _context.Colors.ToList();
            shopVM.Brands = _context.Brands.ToList();
            shopVM.SubCategories = _context.SubCategories.Include(x => x.Category).ToList();
            shopVM.Products = Products.Skip((page - 1) * 8).Take(8).Include(x => x.Brand).Include(x=>x.SubCategory).ThenInclude(x=>x.Category).ToList();
            shopVM.Categories = _context.Categories.ToList();
            shopVM.AllProducts = _context.Products.Include(x=>x.ProductImages).Include(x => x.Brand).Include(x => x.SubCategory).ThenInclude(x => x.Category).Include(x => x.Color).ToList();
            return View(shopVM);
        }
        public IActionResult AddBasket(int id, int count=1)
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
                string productIdsStr = HttpContext.Request.Cookies["basket"];
                List<BasketItemViewModel> items = new List<BasketItemViewModel>();

                if (!string.IsNullOrWhiteSpace(productIdsStr))
                {
                    items = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(productIdsStr);
                }
                BasketItemViewModel item = items.FirstOrDefault(x => x.ProductId == id);
                if (item == null)
                {
                    item = new BasketItemViewModel { ProductId = id, Count = count };
                    items.Add(item);
                }
                else
                {
                    item.Count++;
                }
                productIdsStr = JsonConvert.SerializeObject(items);

                HttpContext.Response.Cookies.Append("basket", productIdsStr);
                TempData["Success"] = "Product Added to Basket !";
                return PartialView("_BasketItemPatrial", _getBasket(items));
            }
            else
            {
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(x => x.AppUserId == member.Id && x.ProductId == id);

                if (basketItem == null)
                {
                    basketItem = new BasketItem
                    {
                        AppUserId = member.Id,
                        ProductId = id,
                        CreatedAt = DateTime.UtcNow.AddHours(4),
                        Count = 1,
                    };
                    _context.BasketItems.Add(basketItem);
                }
                else
                {
                    basketItem.Count++;
                }
                _context.SaveChanges();
                var items = _context.BasketItems.Where(x => x.AppUserId == member.Id).ToList();
                TempData["Success"] = "Product Added to Basket !";
                return PartialView("_BasketItemPatrial", _getBasket(items));
            }


        }
        private BasketViewModel _getBasket(List<BasketItemViewModel> basketItems)
        {
            BasketViewModel basketVM = new BasketViewModel
            {
                BasketProducts = new List<ProductBasketItemViewModel>(),
                TotalPrice = 0
            };

            foreach (var item in basketItems)
            {
                Product product = _context.Products.Include(x => x.Brand).Include(x=>x.ProductImages).Include(x=>x.SubCategory).Include(x=>x.SubCategory).Include(x=>x.Color).FirstOrDefault(x => x.Id == item.ProductId);
                ProductBasketItemViewModel productBasketItem = new ProductBasketItemViewModel
                {
                    Product = product,
                    Count = item.Count
                };

                basketVM.BasketProducts.Add(productBasketItem);
                decimal totalPrice = product.DiscountPercent > 0 ? (product.SalePrice * (1 - product.DiscountPercent / 100)) : product.SalePrice;
                basketVM.TotalPrice += totalPrice * item.Count;
            }

            return basketVM;
        }
        private BasketViewModel _getBasket(List<BasketItem> basketItems)
        {
            BasketViewModel basketVM = new BasketViewModel
            {
                BasketProducts = new List<ProductBasketItemViewModel>(),
                TotalPrice = 0
            };

            foreach (var item in basketItems)
            {
                Product product = _context.Products.Include(x => x.Brand).Include(x=>x.ProductImages).Include(x=>x.SubCategory).ThenInclude(x=>x.Category).FirstOrDefault(x => x.Id == item.ProductId);
                ProductBasketItemViewModel productBasketItem = new ProductBasketItemViewModel
                {
                    Product = product,
                    Count = item.Count
                };

                basketVM.BasketProducts.Add(productBasketItem);
                decimal totalPrice = product.DiscountPercent > 0 ? (product.SalePrice * (1 - product.DiscountPercent / 100)) : product.SalePrice;
                basketVM.TotalPrice += totalPrice * item.Count;
            }

            return basketVM;
        }
        public IActionResult GetCookie(string key)
        {
            var value = HttpContext.Request.Cookies[key];
            return Content(value);
        }
        public IActionResult RemoveBasket(int id)
        {
            if (!_context.Products.Any(x => x.Id == id))
            {
                return NotFound();
            }

            AppUser appUser = null;

            if (User.Identity.IsAuthenticated)
            {
                appUser = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);
            }

            if (appUser == null)
            {
                string cookie = HttpContext.Request.Cookies["basket"];
                List<BasketItemViewModel> cookieItems = new List<BasketItemViewModel>();

                if (!string.IsNullOrWhiteSpace(cookie))
                {
                    cookieItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(cookie);
                }

                BasketItemViewModel cookieItem = cookieItems.FirstOrDefault(x => x.ProductId == id);


                if (cookieItem.Count > 1)
                {
                    cookieItem.Count--;
                }
                else
                {
                    cookieItems.Remove(cookieItem);
                }
                cookie = JsonConvert.SerializeObject(cookieItems);
                HttpContext.Response.Cookies.Append("basket", cookie);
                TempData["Success"] = "Product Removed From Basket !";
                return PartialView("_BasketItemPatrial", _getBasket(cookieItems));
            }
            else
            {
                BasketItem item = _context.BasketItems.FirstOrDefault(x => x.AppUserId == appUser.Id && x.ProductId == id);

                if (item.Count > 1)
                {
                    item.Count--;
                }
                else
                {
                    _context.BasketItems.Remove(item);
                }
                _context.SaveChanges();

                var items = _context.BasketItems.Where(x => x.AppUserId == appUser.Id).ToList();
                TempData["Success"] = "Product Removed From Basket !";
                return PartialView("_BasketItemPatrial", _getBasket(items));
            }


        }
        public IActionResult Search(string searchStr)
        {
            var Products = _context.Products.Include(x => x.ProductImages).Include(x => x.Brand).Include(x => x.Color).AsQueryable();
            if (!String.IsNullOrEmpty(searchStr))
            {
                Products = Products.Where(s => s.Name.Trim().ToLower().Contains(searchStr.Trim().ToLower()) || s.Brand.Name.Trim().ToLower().Contains(searchStr));
            }
            else
            {
                    Products = null;
            }
            return PartialView("_SearchproductPartial", Products.ToList());
        }

    }
}
