using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AnyaStore.Services.ShoppingCartAPI.Models.DTO
{
    public class CartDetailsDTO
    {
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; } // foreign key
        public int ProductId { get; set; } // foreign key
        public int Count { get; set; }
        public CartHeaderDTO? CartHeader { get; set; } // included property
        public ProductDTO? Product { get; set; } // included property
    }
}