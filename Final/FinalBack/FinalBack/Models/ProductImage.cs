using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalBack.Models
{
    public class ProductImage:BaseEntity
    {
        [StringLength(maximumLength: 100)]
        public string Image { get; set; }
        public int ProductId { get; set; }
        public bool? ThumbStatus { get; set; }


    }
}
