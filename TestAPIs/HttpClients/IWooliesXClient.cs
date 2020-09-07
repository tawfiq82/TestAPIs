using System.Threading.Tasks;

namespace TestAPIs.HttpClients
{
    public interface IWooliesXClient
    {
        Task<T> GetAsync<T>(string requestUrl) where T : new();
        Task<T> PostAsync<T>(string requestUrl, object body = null) where T : new();
    }
}