using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnyaStore.Web.Models.DTO
{
    public class ProductDTO
    {
        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int Count { get; set; } = 1;
        public CategoryDTO? Category { get; set; }
    }
}