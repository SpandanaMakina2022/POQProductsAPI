using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Product.Filter.Abstractions
{
    public interface IProductRepository
    {
        Task<List<ProductInfo>> GetProducts();
    }
}
