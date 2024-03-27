using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AnyaStore.Web.Models.DTO
{
    public class CartDetailsRemoveDTO
    {
        public int? CartDetailsId { get; set; }
        public int? CartHeaderId { get; set; } // foreign key
    }
}