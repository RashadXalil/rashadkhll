using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.ViewModels
{
    public class BasketViewModel
    {
        public List<ProductBasketItemViewModel> BasketProducts { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
