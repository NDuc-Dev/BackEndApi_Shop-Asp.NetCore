using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebIdentityApi.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Full name is require")]
        public string FullName { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        [Column(TypeName = "decimal(9,0)")]
        public decimal TotalSpending { get; set; } = 0;
        public int SpendingPoint { get; set; } = 0;
        public bool AccountStatus { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;
        public ICollection<Brand> CreatedBrands { get; set; } = null;
        public ICollection<Product> CreatedProducts { get; set; } = null;
        public ICollection<Order> CreatedOrders { get; set; } = null;

    }
}
