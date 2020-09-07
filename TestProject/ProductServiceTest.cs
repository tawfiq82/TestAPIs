using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TestAPIs.HttpClients;
using TestAPIs.Models;
using TestAPIs.Services;

namespace TestProject
{
    public class ProductServiceTest
    {
        private IProductService _productService;
        private Mock<IWooliesXClient> _wooliesXClientMock;
        private Mock<ILogger<ProductService>> _loggerMock;

        private List<Product> mockProducts = new List<Product>() { new Product() { Name = "Product A", Price = 10 }, new Product() { Name = "Product B", Price = 20 }, new Product() { Name = "Product C", Price = 30 } };

        private List<ShoppingOrder> mockShoppingOrders = new List<ShoppingOrder>
        {
            new ShoppingOrder()
            {
                CustomerId = 1,
                Products= new List<Product>()
                {
                    new Product()
                    {
                        Name = "Product A",
                        Price = 10,
                        Quantity=1
                    },
                    new Product()
                    {
                        Name = "Product B",
                        Price = 20,
                        Quantity=1
                    },
                     new Product()
                    {
                        Name = "Product C",
                        Price = 30,
                        Quantity=1
                    }
                }
            },
             new ShoppingOrder()
            {
                CustomerId = 2,
                Products= new List<Product>()
                {
                    new Product()
                    {
                        Name = "Product C",
                        Price = 10,
                        Quantity=1
                    },
                    new Product()
                    {
                        Name = "Product B",
                        Price = 20,
                        Quantity=1
                    }
                }
            },
              new ShoppingOrder()
            {
                CustomerId = 3,
                Products= new List<Product>()
                {
                    new Product()
                    {
                        Name = "Product B",
                        Price = 10,
                        Quantity=1
                    }
                }
            }
        };

        [SetUp]
        public void Setup()
        {
            _wooliesXClientMock = new Mock<IWooliesXClient>();
            _loggerMock = new Mock<ILogger<ProductService>>();

            _wooliesXClientMock.Setup(x => x.GetAsync<List<Product>>("products")).ReturnsAsync(mockProducts);
            _wooliesXClientMock.Setup(x => x.GetAsync<List<ShoppingOrder>>("shopperHistory")).ReturnsAsync(mockShoppingOrders);

            _productService = new ProductService(_wooliesXClientMock.Object, _loggerMock.Object);
        }

        [Test]
        public void TestSortLow()
        {
            var sortedProducts = _productService.GetProductsAsync("Low").Result;

            var exprectedResults = mockProducts.OrderBy(x => x.Price).ToList();

            for (int i = 0; i < sortedProducts.Count; i++)
            {
                Assert.AreEqual(exprectedResults[i].Name, sortedProducts[i].Name, "Products are sortes in order Low price");
            }
        }

        [Test]
        public void TestSortHigh()
        {
            var sortedProducts = _productService.GetProductsAsync("High").Result;

            var exprectedResults = mockProducts.OrderByDescending(x => x.Price).ToList();

            for (int i = 0; i < sortedProducts.Count; i++)
            {
                Assert.AreEqual(exprectedResults[i].Name, sortedProducts[i].Name, "Products are sortes in order High price");
            }
        }

        [Test]
        public void TestSortAscending()
        {
            var sortedProducts = _productService.GetProductsAsync("Ascending").Result;

            var exprectedResults = mockProducts.OrderBy(x => x.Name).ToList();

            for (int i = 0; i < sortedProducts.Count; i++)
            {
                Assert.AreEqual(exprectedResults[i].Name, sortedProducts[i].Name, "Products are sortes in order Ascending price");
            }
        }

        [Test]
        public void TestSortDescending()
        {
            var sortedProducts = _productService.GetProductsAsync("Descending").Result;

            var exprectedResults = mockProducts.OrderByDescending(x => x.Name).ToList();

            for (int i = 0; i < sortedProducts.Count; i++)
            {
                Assert.AreEqual(exprectedResults[i].Name, sortedProducts[i].Name, "Products are sortes in order Descending price");
            }
        }

        [Test]
        public void TestSortRecommended()
        {
            var sortedProducts = _productService.GetProductsAsync("Recommended").Result;

            foreach (var item in mockProducts)
            {
                item.PopularityScore = mockShoppingOrders.SelectMany(x => x.Products).Where(x => x.Name == item.Name).Count();
            }

            var exprectedResults = mockProducts.OrderByDescending(x => x.PopularityScore).ToList();

            for (int i = 0; i < sortedProducts.Count; i++)
            {
                Assert.AreEqual(exprectedResults[i].Name, sortedProducts[i].Name, "Products are sortes in order Recommended price");
            }
        }
    }
}