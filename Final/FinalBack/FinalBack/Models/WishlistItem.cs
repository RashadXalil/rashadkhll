using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalBack.Models
{
    public class WishlistItem:BaseEntity
    {
        public string AppUserId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }

        public AppUser AppUser { get; set; }
        public Product Product { get; set; }
    }
}
