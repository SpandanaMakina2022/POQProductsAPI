using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Product.Filter.Abstractions
{
    /// <summary>
    /// Product properties derived from the mocky.io database
    /// </summary>
    public class ProductInfo
    {
        [JsonProperty(PropertyName = "title", Order = 1)]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "price", Order = 2)]
        public int Price { get; set; }
        [JsonProperty(PropertyName = "description", Order = 4)]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "sizes", Order = 3)]
        public List<string> Sizes { get; set; }
    }
}
