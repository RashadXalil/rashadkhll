using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalBack.Models
{
    public class Color:BaseEntity
    {
        [StringLength(maximumLength:50)]
        public string Name { get; set; }
        [StringLength(maximumLength: 50)]
        public string HexCode { get; set; }
    }
}
