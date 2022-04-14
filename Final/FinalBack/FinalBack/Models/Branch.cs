using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalBack.Models
{
    public class Branch:BaseEntity
    {
        [StringLength(maximumLength:200)]
        public string Image { get; set; }
        [StringLength(maximumLength: 100)]

        public string Name { get; set; }
        [StringLength(maximumLength: 200)]

        public string Location { get; set; }
        [StringLength(maximumLength: 200)]

        public string Phone { get; set; }
        [StringLength(maximumLength: 50)]

        public string Mail { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
