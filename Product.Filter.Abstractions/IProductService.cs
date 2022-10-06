using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Product.Filter.Abstractions
{
    public interface IProductService
    {
        Task<IEnumerable<ProductInfo>> Filter(QueryParams queryParams);
        
    }
}
