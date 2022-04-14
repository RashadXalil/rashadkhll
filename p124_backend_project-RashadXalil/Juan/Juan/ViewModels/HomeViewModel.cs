using Juan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; }
        public List<Features> Features { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Category> Categories { get; set; }
        public List<Color> Colors { get; set; }
        public Product Product { get; set; }
        public List<Product> Products { get; set; }
        public List<ProductColor> ProductColors { get; set; }
        public List<Gender> Genders { get; set; }
        public Dictionary<string,string> Settings { get; set; }
        public Review Review { get; set; }
        public List<Product> RatedProducts { get; set; }
    }
}
