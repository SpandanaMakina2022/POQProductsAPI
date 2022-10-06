using Microsoft.Extensions.Logging;
using Product.Filter.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Filter.Service
{
    /// <summary>
    /// Product service provides the business logic and the code is scalable and secured from the abstract layer
    /// </summary>
    public class ProductService : IProductService
    {

        private readonly ILogger<ProductService> logger;
        private readonly IProductRepository productRepository;
                
        public ProductService(
            ILogger<ProductService> logger,
            IProductRepository productRepository)
        {
            this.logger = logger;
            this.productRepository = productRepository;
        }

        
        public async Task<IEnumerable<ProductInfo>> Filter(QueryParams queryParams)
        {
            try
            {
                List<ProductInfo> products = await this.productRepository.GetProducts();
                
                if (products?.Count > 0)
                {
                    //applying the 
                    var filter = products.AsQueryable();

                    if (queryParams.MinPrice >= 0)
                    {
                        filter = filter.Where(p => p.Price >= queryParams.MinPrice);
                    }
                    if (queryParams.MaxPrice >= 0)
                    {
                        filter = filter.Where(p => p.Price <= queryParams.MaxPrice);
                    }
                    if (!string.IsNullOrEmpty(queryParams.Size))
                    {
                        filter = filter.Where(p => p.Sizes.Any(s => s.Equals(queryParams.Size, StringComparison.OrdinalIgnoreCase)));
                    }
                    if (!string.IsNullOrEmpty(queryParams.Highlight))
                    {
                        filter =  ApplyHightlight(filter, queryParams.Highlight);
                    }

                    return filter.AsEnumerable();
                }

                return null;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error - {ex.Message}", ex);
                throw ex;
            }

        }


        private IQueryable<ProductInfo> ApplyHightlight(IQueryable<ProductInfo> filter, string highlight)
        {

            var excludeTexts = new List<string>() { "this", "perfectly", "pairs", "with", "a" };
            var highlightList = highlight.ToLower().Split(',').Where(h => !excludeTexts.Contains(h) && !string.IsNullOrEmpty(h));
            foreach (var word in highlightList)
            {
                filter = filter.Select(f => new ProductInfo()
                {
                    Description = f.Description.Replace(word, $"<em>{word}</em>"),
                    Price = f.Price,
                    Sizes = f.Sizes,
                    Title = f.Title
                });
            }

            return filter;
        }
    }
}
