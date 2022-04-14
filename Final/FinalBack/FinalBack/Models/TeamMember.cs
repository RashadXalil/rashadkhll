using FinalBack.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalBack.Models
{
    public class TeamMember:BaseEntity
    {
        public string Image { get; set; }
        [Required]
        [StringLength(maximumLength:200)]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength:200)]
        public string Surname { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string Position { get; set; }
        [StringLength(maximumLength: 200)]
        public string FacebookURL { get; set; }
        [StringLength(maximumLength: 200)]
        public string TwitterURl { get; set; }
        [StringLength(maximumLength: 200)]
        public string DribbleURL { get; set; }
        [StringLength(maximumLength: 200)]
        public string YoutubeURL { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }


    }
}
