using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Product.Filter.Abstractions
{
    public class ProductsResponse
    {
        [JsonProperty(PropertyName = "products", Order = 1)]
        public List<ProductInfo> Products { get; set; }
    }
}
