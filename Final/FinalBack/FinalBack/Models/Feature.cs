using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalBack.Models
{
    public class Feature:BaseEntity
    {
        [Required]
        [StringLength(maximumLength: 200)]
        public string Image { get; set; }
        [Required]
        [StringLength(maximumLength:200)]
        public string Title { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string Description { get; set; }
    }
}
