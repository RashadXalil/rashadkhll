using FinalBack.Models;
using FinalBack.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace FinalBack.Services
{
    public class LayoutService
    {
        private readonly FinalContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LayoutService(FinalContext context, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public Dictionary<string, string> GetSettings()
        {
            return _context.Settings.ToDictionary(x => x.Key, x => x.Value);
        }
        public List<Category> getCategories()
        {
            return _context.Categories.Where(x => x.IsDeleted == false).ToList();
        }
        public List<SubCategory> getSubCategories()
        {
            return _context.SubCategories.Include(x => x.Category).Where(x => x.IsDeleted == false).ToList();
        }
        public BasketViewModel GetBasket()
        {
            BasketViewModel basketVM = new BasketViewModel
            {

                BasketProducts = new List<ProductBasketItemViewModel>(),
                TotalPrice = 0,
            };

            List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();

            AppUser member = null;

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == _httpContextAccessor.HttpContext.User.Identity.Name && !x.IsAdmin);


            }
            if (member == null)
            {
                var basketstr = _httpContextAccessor.HttpContext.Request.Cookies["basket"];

                if (!string.IsNullOrWhiteSpace(basketstr))
                {
                    basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketstr);
                }
            }
            else
            {
                basketItems = _context.BasketItems.Where(x => x.AppUserId == member.Id).Select(b => new BasketItemViewModel { ProductId = b.ProductId, Count = b.Count }).ToList();
            }
            foreach (var item in basketItems)
            {
                Product product = _context.Products.Include(x=>x.ProductImages).Include(x=>x.SubCategory).ThenInclude(x=>x.Category).Include(x=>x.Color).Include(x => x.Brand).FirstOrDefault(x => x.Id == item.ProductId);

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
    }
}
