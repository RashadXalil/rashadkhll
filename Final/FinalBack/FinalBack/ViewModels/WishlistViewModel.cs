using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalBack.ViewModels
{
    public class WishlistViewModel
    {
        public List<ProductWishlistItemViewModel> WishlistProducts { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
