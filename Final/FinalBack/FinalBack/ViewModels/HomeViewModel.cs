using FinalBack.Models;
using System.Collections.Generic;

namespace FinalBack.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<BlogCategory> BlogCategories { get; set; }
        public List<Blog> PopularBlogs { get; set; }
        public List<Faq> LeftFaqs { get; set; }
        public List<Faq> RightFaqs { get; set; }
        public Dictionary<string, string> Settings { get; set; }
        public List<Feature> Features { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Product> HotTrendingProducts { get; set; }
        public Product Product { get; set; }
        public List<Product> TopDealProducts { get; set; }
        public Blog Blog { get; set; }
        public List<Blog> RelatedBlogs { get; set; }
        public List<Category> PopularCategories { get; set; }
        public List<Product> AllProducts { get; set; }
        public List<Product> RecommendingProducts { get; set; }
        public List<Product> TopFeaturedProductBanner { get; set; }
        public List<Product> TopFeaturedProducts { get; set; }
        public List<Service> Services { get; set; }
        public List<Branch> Branches { get; set; }
        public List<TeamMember> Members { get; set; }
        public Review Review { get; set; }
    }
}
