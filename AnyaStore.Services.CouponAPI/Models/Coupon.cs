using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnyaStore.Services.CouponAPI.Models
{
    public class Coupon
    {
        public int CouponId { get; set; }
        
        public string? CouponCode { get; set; }
        
        public double? DiscountAmount { get; set; }

        public int? MinAmount { get; set; }

        public DateTime? LastUpdated { get; set; }

        public DateTime? CreatedOn { get; set; }

        
        
    }
}