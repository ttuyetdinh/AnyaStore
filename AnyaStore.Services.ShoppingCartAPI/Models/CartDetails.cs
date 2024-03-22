using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AnyaStore.Services.ShoppingCartAPI.Models.DTO;

namespace AnyaStore.Services.ShoppingCartAPI.Models
{
    public class CartDetails
    {
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; } // foreign key
        public int ProductId { get; set; } // foreign key
        public int Count { get; set; }
        public int Order { get; set; }
        public CartHeader CartHeader { get; set; } // included property
        [NotMapped]
        public ProductDTO Product { get; set; } // included property
        public DateTime? LastUpdated { get; set; } = DateTime.Now;
        public DateTime? CreatedOn { get; set; } = DateTime.Now;
    }
}