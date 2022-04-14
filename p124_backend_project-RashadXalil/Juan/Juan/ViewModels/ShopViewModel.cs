using Juan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.ViewModels
{
    public class ShopViewModel
    {
        public List<Brand> Brands { get; set; }
        public List<Category> Categories { get; set; }
        public List<Color> Colors { get; set; }
        public List<Product> Products { get; set; }
        public List<ProductColor> ProductColors { get; set; }
        public List<int> filterColorIds { get; set; }
        public List<Gender> Genders { get; set; }
        public Dictionary<string, string> Settings { get; set; }
        public List<Product> AllProducts { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
