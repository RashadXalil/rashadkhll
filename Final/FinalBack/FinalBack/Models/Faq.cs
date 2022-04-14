using System.ComponentModel.DataAnnotations;

namespace FinalBack.Models
{
    public class Faq:BaseEntity
    {
        [Required]
        [StringLength(maximumLength:200)]
        public string FaqHeader { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string FaqBody { get; set; } 

    }
}
