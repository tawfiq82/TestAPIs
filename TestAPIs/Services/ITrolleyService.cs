using System.Threading.Tasks;
using TestAPIs.Models;

namespace TestAPIs.Services
{
    public interface ITrolleyService
    {
        decimal GetTrolleyTotal(TrolleyItems trolleyItems);

        Task<decimal> GetTrolleyTotalX(TrolleyItems trolleyItems);
    }
}