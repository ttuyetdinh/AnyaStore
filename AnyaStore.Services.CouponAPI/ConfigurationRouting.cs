using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace AnyaStore.Services.CouponAPI
{
    public class LowercaseControllerParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            // Replace "DeliveryCategories" with "delivery-categories"
            return value == null ? null : Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}