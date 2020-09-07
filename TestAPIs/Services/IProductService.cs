using System.Collections.Generic;
using System.Threading.Tasks;
using TestAPIs.Models;

namespace TestAPIs.Services
{
    public interface IProductService
    {
        Task<IList<Product>> GetProductsAsync(string sortOption = "");
    }
}