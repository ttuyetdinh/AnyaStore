using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnyaStore.Web.Models.DTO
{
    public class CouponDTO
    {
        public int CouponId { get; set; }

        [Required]
        public string? CouponCode { get; set; }

        public double? DiscountAmount { get; set; }

        public int? MinAmount { get; set; }
    }
}