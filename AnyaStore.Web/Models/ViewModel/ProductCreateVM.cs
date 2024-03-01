using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Web.Models.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnyaStore.Web.Models.ViewModel
{
    public class ProductCreateVM
    {
        public ProductCreateVM()
        {
            Product = new ProductDTO();
        }
        public ProductDTO Product { get; set; }

        // this field will be null when submit to the controller, so ignore it from validation
        [ValidateNever]
        public IEnumerable<SelectListItem>? Categories { get; set; }
    }
}