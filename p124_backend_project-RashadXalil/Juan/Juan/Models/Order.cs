using Juan.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AppUserId { get; set; }
        [Required]
        [StringLength(maximumLength:50)]
        public string FullName { get; set; }
        [StringLength(maximumLength: 150)]
        public string Email { get; set; }
        [Required]

        [StringLength(maximumLength: 50)]
        public string PhoneNumber { get; set; }
        [Required]

        [StringLength(maximumLength: 50)]
        public string Country { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string City { get; set; }
        [Required]
        [StringLength(maximumLength: 150)]
        public string Address { get; set; }
        public OrderStatus Status { get; set; }  
        [Column(TypeName ="decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        [StringLength(maximumLength:300)]
        public string Note { get; set; }

        public List<OrderItem> OrderItems { get; set; }
        public AppUser AppUser { get; set; }

    }
}
