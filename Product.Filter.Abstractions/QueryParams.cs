using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Product.Filter.Abstractions
{
    /// <summary>
    /// Properties created based on the filters asked on this project
    /// </summary>
    public class QueryParams
    {
        [JsonProperty(PropertyName = "minPrice", Order = 1)]
        public double? MinPrice { get; set; }
        [JsonProperty(PropertyName = "maxPrice", Order = 2)]
        public double? MaxPrice { get; set; }
        [JsonProperty(PropertyName = "size", Order = 3)]
        public string Size { get; set; }
        [JsonProperty(PropertyName = "highlight", Order = 4)]
        public string Highlight { get; set; }
    }
}
