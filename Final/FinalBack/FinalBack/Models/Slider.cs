using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalBack.Models
{
    public class Slider:BaseEntity
    {
        [Required]
        [StringLength(maximumLength: 200)]
        public string Image { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string Title1 { get; set; }
        [StringLength(maximumLength: 100)]
        public string Title2 { get; set; }
        [StringLength(maximumLength: 200)]
        public string Desc { get; set; }

        [StringLength(maximumLength: 50)]
        public string BtnText { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
