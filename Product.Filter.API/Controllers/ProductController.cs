using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Product.Filter.Abstractions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Product.Filter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private  IProductService _productService { get; }

        //Dependency injection for logger and product service
        public ProductController(
            IProductService productService,
            ILogger<ProductController> logger)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet("Filter")]
        public async Task<ActionResult<IEnumerable<ProductInfo>>> GetProductsByFilter([FromQuery]QueryParams queryParams)
        {
            try
            {
                var filteredproducts = await _productService.Filter(queryParams);

                if(filteredproducts is null)
                {
                    return NotFound();
                }

                return Ok(filteredproducts);
            }
            catch (Exception)
            {
               return StatusCode((int)HttpStatusCode.InternalServerError, new { created = DateTime.UtcNow });
            }
        }

        
    }
}
