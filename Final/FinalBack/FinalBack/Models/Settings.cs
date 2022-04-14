using System.ComponentModel.DataAnnotations;

namespace FinalBack.Models
{
    public class Settings
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 200)]

        public string Key { get; set; }
        [StringLength(maximumLength: 200)]

        public string Value { get; set; }
    }
}
