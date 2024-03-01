using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace AnyaStore.Services.ProductAPI.Models.DTO
{
    public class CategoryDTO
    {
        [Required]
        public int CategoryId { get; set; }
        public string Name { get; set; }

    }
}