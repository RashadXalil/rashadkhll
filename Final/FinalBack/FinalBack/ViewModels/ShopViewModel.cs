using FinalBack.Models;
using System.Collections.Generic;

namespace FinalBack.ViewModels
{
    public class ShopViewModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Color> Colors { get; set; }
        public List<Product> AllProducts { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
