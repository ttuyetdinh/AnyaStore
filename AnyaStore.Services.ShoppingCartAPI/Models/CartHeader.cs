using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnyaStore.Services.ShoppingCartAPI.Models
{
    public class CartHeader
    {
        public int CartHeaderId { get; set; }
        public string? UserId { get; set; } // foreign key
        public string? CouponCode { get; set; }
        [NotMapped]
        public double Discount { get; set; }
        [NotMapped]
        public double CartTotal { get; set; }
        public DateTime? LastUpdated { get; set; } = DateTime.Now;
        public DateTime? CreatedOn { get; set; } = DateTime.Now;
        public ICollection<CartDetails> CartDetails { get; set; }
    }
}