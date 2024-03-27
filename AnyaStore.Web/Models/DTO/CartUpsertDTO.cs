using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnyaStore.Web.Models.DTO
{
    public class CartUpsertDTO
    {
        public CartHeaderDTO CartHeader { get; set; }
        public IEnumerable<CartDetailsUpsertDTO>? CartDetails { get; set; }
    }
}