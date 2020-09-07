using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestAPIs.HttpClients;
using TestAPIs.Models;

namespace TestAPIs.Services
{
    public class TrolleyService : ITrolleyService
    {
        private readonly IWooliesXClient _wooliesXClient;
        private readonly ILogger<TrolleyService> _logger;

        public TrolleyService(IWooliesXClient wooliesXClient, ILogger<TrolleyService> logger)
        {
            _wooliesXClient = wooliesXClient;
            _logger = logger;
        }

        public decimal GetTrolleyTotal(TrolleyItems trolleyItems)
        {
            if (trolleyItems.Products == null || trolleyItems.Products.Count == 0)
            {
                throw new Exception("No products are specified.");
            }

            if (trolleyItems.Quantities == null || trolleyItems.Quantities.Count == 0)
            {
                throw new Exception("No quantites are specified.");
            }


            decimal totalPrice = 0;

            foreach (var quantityItem in trolleyItems.Quantities)
            {
                decimal trolleyProductPrice = 0;
                var product = trolleyItems.Products.FirstOrDefault(p => p.Name == quantityItem.Name);

                if (product == null)
                {
                    throw new Exception("Failed to find matching product.");
                }

                var quantityToCalculate = quantityItem.Quantity;

                var specialItem = trolleyItems.Specials.FirstOrDefault(y => y.Quantities.Any(z => z.Name == quantityItem.Name));
                if (specialItem != null)
                {
                    var specialQunatityItem = specialItem.Quantities.First(x => x.Name == quantityItem.Name);
                    var actualPriceForSpecialQuanity = product.Price * specialQunatityItem.Quantity;

                    if (specialItem.Total < actualPriceForSpecialQuanity)
                    {
                        while (quantityToCalculate > specialQunatityItem.Quantity)
                        {
                            quantityToCalculate -= specialQunatityItem.Quantity;
                            trolleyProductPrice += specialItem.Total;
                        }
                    }
                }

                trolleyProductPrice += quantityToCalculate * product.Price;

                totalPrice += trolleyProductPrice;
            }
            return totalPrice;
        }

        public async Task<decimal> GetTrolleyTotalX(TrolleyItems trolleyItems)
        {
            try
            {
                var totalPrice = await _wooliesXClient.PostAsync<decimal>("trolleyCalculator", trolleyItems);
                return totalPrice;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
