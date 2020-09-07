using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestAPIs.HttpClients;
using TestAPIs.Models;

namespace TestAPIs.Services
{
    public class ProductService : IProductService
    {
        private readonly IWooliesXClient _wooliesXClient;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IWooliesXClient wooliesXClient, ILogger<ProductService> logger)
        {
            _wooliesXClient = wooliesXClient;
            _logger = logger;
        }

        public async Task<IList<Product>> GetProductsAsync(string sortOption = "")
        {
            try
            {
                var products = await _wooliesXClient.GetAsync<List<Product>>("products");

                if (string.IsNullOrEmpty(sortOption))
                    return products;

                switch (sortOption.Trim())
                {
                    case "Low":
                        return products.OrderBy(p => p.Price).ToList();
                    case "High":
                        return products.OrderByDescending(p => p.Price).ToList();
                    case "Ascending":
                        return products.OrderBy(p => p.Name).ToList();
                    case "Descending":
                        return products.OrderByDescending(p => p.Name).ToList();
                    case "Recommended":
                        var shoppingOrders = await _wooliesXClient.GetAsync<List<ShoppingOrder>>("shopperHistory");
                        foreach (var item in products)
                        {
                            item.PopularityScore = shoppingOrders.SelectMany(x => x.Products).Where(x => x.Name == item.Name).Count();
                        }
                        return products.OrderByDescending(p => p.PopularityScore).ToList();
                }

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to collect products.");
                return null;
            }
        }
    }
}
