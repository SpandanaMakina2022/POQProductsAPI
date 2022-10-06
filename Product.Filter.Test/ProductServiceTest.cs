using FakeItEasy;
using Microsoft.Extensions.Logging;
using Product.Filter.Abstractions;
using Product.Filter.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Product.Filter.Test
{
    /// <summary>
    /// Used Xunit framework to write unit tests and FakeItEasy library for mocking fake data
    /// </summary>
    public class ProductServiceTest
    {
        //Fake test data
        List<ProductInfo> fakeProducts = new List<ProductInfo>()
        {
            new ProductInfo()
            {
                Description = "This hat perfectly pairs with a red tie",
                Price = 5,
                Sizes = new List<string>(){"small", "large"},
                Title = "Product 101"
            },
            new ProductInfo()
            {
                Description = "This hat perfectly pairs with a green tie",
                Price = 20,
                Sizes = new List<string>(){"small", "large", "medium"},
                Title = "Product 102"
            },
            new ProductInfo()
            {
                Description = "This hat perfectly pairs with a blue shoe",
                Price = 10,
                Sizes = new List<string>(){"small", "medium"},
                Title = "Product 103"
            }
        };

        [Fact]
        public void ProductService_Filter_Return_AllProducts()
        {
            // Arrange
            var logger = A.Fake<ILogger<ProductService>>();
            var repo = A.Fake<IProductRepository>();
            A.CallTo(() => repo.GetProducts()).Returns(Task.FromResult(fakeProducts));
            var service = new ProductService(logger, repo);

            // Act
            var filteredProducts = service.Filter(new QueryParams() { Highlight = null, MaxPrice = null, MinPrice = null, Size = null }).Result;


            // Assert
            Assert.Equal(fakeProducts.Count, filteredProducts.Count());
        }


        [Fact]
        public void ProductService_Filter_MinPrice_Return_MinPriceProducts()
        {
            // Arrange
            var logger = A.Fake<ILogger<ProductService>>();
            var repo = A.Fake<IProductRepository>();
            A.CallTo(() => repo.GetProducts()).Returns(Task.FromResult(fakeProducts));
            var service = new ProductService(logger, repo);

            // Act
            var filteredProducts = service.Filter(new QueryParams() { Highlight = null, MaxPrice = null, MinPrice = 10, Size = null }).Result;

            var realCount = fakeProducts.Where(f => f.Price >= 10).Count();
            // Assert
            Assert.Equal(realCount, filteredProducts.Count());
        }

        [Fact]
        public void ProductService_Filter_MaxPrice_Return_Products()
        {
            // Arrange
            var logger = A.Fake<ILogger<ProductService>>();
            var repo = A.Fake<IProductRepository>();
            A.CallTo(() => repo.GetProducts()).Returns(Task.FromResult(fakeProducts));
            var service = new ProductService(logger, repo);

            // Act
            var filteredProducts = service.Filter(new QueryParams() { Highlight = null, MaxPrice = 10, MinPrice = null, Size = null }).Result;

            var realCount = fakeProducts.Where(f => f.Price <= 10).Count();
            // Assert
            Assert.Equal(realCount, filteredProducts.Count());
        }

        [Fact]
        public void ProductService_Filter_Size_Return_Products()
        {
            // Arrange
            var logger = A.Fake<ILogger<ProductService>>();
            var repo = A.Fake<IProductRepository>();
            A.CallTo(() => repo.GetProducts()).Returns(Task.FromResult(fakeProducts));
            var service = new ProductService(logger, repo);

            // Act
            var filteredProducts = service.Filter(new QueryParams() { Highlight = null, MaxPrice = null, MinPrice = null, Size = "small" }).Result;

            var realCount = fakeProducts.Where(f => f.Sizes.Contains("small")).Count();
            // Assert
            Assert.Equal(realCount, filteredProducts.Count());
        }

        [Fact]
        public void ProductService_Filter_Highlight_Return_Products()
        {
            // Arrange
            var logger = A.Fake<ILogger<ProductService>>();
            var repo = A.Fake<IProductRepository>();
            A.CallTo(() => repo.GetProducts()).Returns(Task.FromResult(fakeProducts));
            var service = new ProductService(logger, repo);

            // Act
            var filteredProducts = service.Filter(new QueryParams() { Highlight = "blue,green", MaxPrice = null, MinPrice = null, Size = null }).Result;

            var highlightTexts = "blue,green".Split(',');

            foreach (var word in highlightTexts)
            {
                fakeProducts = fakeProducts.Select(f =>
                    new ProductInfo()
                    {
                        Description = f.Description.Replace(word, $"<em>{word}</em>"),
                        Sizes = f.Sizes,
                        Title = f.Title,
                        Price = f.Price
                    }).ToList();
            }

            // Assert
            foreach (var fProduct in fakeProducts)
            {
                Assert.Equal(
                    fakeProducts.Where(f => f.Description == fProduct.Description).Count(),
                    filteredProducts.Where(p => p.Description == fProduct.Description).Count());
            }

        }

        [Fact]
        public void ProductService_Filter_AllFilters_Return_Products()
        {
            // Arrange
            var logger = A.Fake<ILogger<ProductService>>();
            var repo = A.Fake<IProductRepository>();
            A.CallTo(() => repo.GetProducts()).Returns(Task.FromResult(fakeProducts));
            var service = new ProductService(logger, repo);

            // Act
            var filteredProducts = service.Filter(new QueryParams() { Highlight = "blue,green", MaxPrice = 20, MinPrice = 5, Size = "large" }).Result;

            var realCount = fakeProducts.Where(f => f.Price >= 5 && f.Price <= 20 &&  f.Sizes.Contains("large")).Count();
            fakeProducts = fakeProducts.Where(f => f.Price >= 5 && f.Price <= 20 && f.Sizes.Contains("large")).ToList();

            var highlightTexts = "blue,green".Split(',');
            foreach (var word in highlightTexts)
            {
                fakeProducts = fakeProducts.Select(f =>
                    new ProductInfo()
                    {
                        Description = f.Description.Replace(word, $"<em>{word}</em>"),
                        Sizes = f.Sizes,
                        Title = f.Title,
                        Price = f.Price
                    }).ToList();
            }

            // Assert

            //For filters
            Assert.Equal(realCount, filteredProducts.Count());

            // For Highlight
            foreach (var fProduct in fakeProducts)
            {
                Assert.Equal(
                    fakeProducts.Where(f => f.Description == fProduct.Description).Count(),
                    filteredProducts.Where(p => p.Description == fProduct.Description).Count());
            }
            
        }
    }
}
