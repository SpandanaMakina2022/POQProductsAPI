using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Product.Filter.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Product.Filter.Repository
{
    /// <summary>
    /// Repository/Database security layer between business logic (Product service) and the database/repository interaction
    /// </summary>
    public class ProductRepository: IProductRepository
    {
        private readonly ILogger<ProductRepository> logger;

        public ProductRepository(ILogger<ProductRepository> logger)
        {
            this.logger = logger;
        }

        public async Task<List<ProductInfo>> GetProducts()
        {
            try
            {
                var resultString = await GetResults("http://www.mocky.io/v2/5e307edf3200005d00858b49");
                if (string.IsNullOrEmpty(resultString))
                {
                    this.logger.LogWarning("No products found");
                }

                //Response added to the log
                this.logger.LogInformation("mocky.io response ", resultString);
                var result = JsonConvert.DeserializeObject<ProductsResponse>(resultString);

                return result?.Products?.Count > 0 ? result.Products : null;
            }
            catch (Exception ex)
            {
                this.logger.LogError("Error while getting products", ex);
                throw ex;
            }

        }

        private async Task<string> GetResults(string url)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("Error product API", ex);
                throw ex;
            }
        }
    }
}
