using Juan.Models;
using Juan.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Juan.Models.PaginatedList;

namespace Juan.Controllers
{
    public class ShopController : Controller
    {
        private readonly JuanContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ShopController(JuanContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index(int? genderId,int?catId,List<int> colorIds,int?brandId,decimal? minPrice,decimal? maxPrice,int page=1)
        {
            ViewBag.PageIndex = page;
            ViewBag.GenderId = genderId;
            ViewBag.CategoryId = catId;
            ViewBag.colorIds = colorIds;
            ViewBag.brandId = brandId;
            

            ShopViewModel shopVM = new ShopViewModel();

            var products = _context.Products.Include(x => x.Gender).Include(x => x.Category).Include(x=>x.productColors).ThenInclude(x=>x.Color).AsQueryable();
            if(genderId!=null)
            {
                products = products.Where(x => x.GenderId == genderId);
            }
            if(catId!=null)
            {
                products =  products.Where(x => x.CategoryId == catId);
            }
            if (colorIds != null && colorIds.Count > 0)
                products = products.Where(x => x.productColors.Any(bt => colorIds.Contains(bt.ColorId)));
            if (brandId != null)
            {
                products = products.Where(x => x.BrandId == brandId);
            }
            if(products.Any())
            {
            shopVM.MaxPrice = products.Max(x => x.SalePrice);
            shopVM.MinPrice = products.Min(x => x.SalePrice);
            }
            ViewBag.FilterMinPrice = minPrice ?? shopVM.MinPrice;
            ViewBag.FilterMaxPrice = maxPrice ?? shopVM.MaxPrice;
            ViewBag.TotalPage = (int)Math.Ceiling(products.Count() / 6d);

            if (minPrice != null && maxPrice != null)
            {
                products = products.Where(x=>x.DiscountPercent>0? x.SalePrice * (1 - x.DiscountPercent / 100) >= minPrice && x.SalePrice * (1 - x.DiscountPercent / 100) <= maxPrice : x.SalePrice >= minPrice && x.SalePrice<=maxPrice);
            }

            shopVM.filterColorIds = colorIds;
            shopVM.Colors = _context.Colors.ToList();
            shopVM.Brands = _context.Brands.ToList();
            shopVM.Categories = _context.Categories.ToList();
            shopVM.Genders = _context.Genders.ToList();
            shopVM.ProductColors = _context.ProductColors.ToList();
            shopVM.Products = products.Skip((page - 1) * 6).Take(6).Include(x => x.Brand).ToList();
            shopVM.AllProducts = _context.Products.Include(x => x.Gender).Include(x => x.Category).Include(x => x.productColors).ThenInclude(x => x.Color).ToList();
            shopVM.Settings = _context.Settings.ToDictionary(x => x.Key, x => x.Value);
            
             return View(shopVM);
        }

        public IActionResult AddBasket(int id)
        {
            if(!_context.Products.Any(x=>x.Id == id))
            {
                return PartialView("_CustomErrorpartial");
            }

            AppUser member = null;

            if(User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);
            }

            if(member == null)
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
                    item = new BasketItemViewModel { ProductId = id, Count = 1 };
                    items.Add(item);
                }
                else
                {
                    item.Count++;
                }
                productIdsStr = JsonConvert.SerializeObject(items);

                HttpContext.Response.Cookies.Append("basket", productIdsStr);

                return PartialView("_BasketItemPatrial", _getBasket(items));
            }
            else
            {
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(x =>x.AppUserId==member.Id && x.ProductId == id);

                if (basketItem == null)
                {
                    basketItem = new BasketItem
                    {
                        AppUserId = member.Id,
                        ProductId = id,
                        CreatedAt = DateTime.UtcNow.AddHours(4),
                        Count = 1
                    };
                    _context.BasketItems.Add(basketItem);
                }
                else
                {
                    basketItem.Count++;
                }


                _context.SaveChanges();
                var items = _context.BasketItems.Where(x => x.AppUserId == member.Id).ToList();
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
                Product product = _context.Products.Include(x=>x.Brand).FirstOrDefault(x => x.Id == item.ProductId);
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
                Product product = _context.Products.Include(x => x.Brand).FirstOrDefault(x => x.Id == item.ProductId);
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
                return PartialView("_BasketItemPatrial", _getBasket(items));
            }


        }

    }
}
