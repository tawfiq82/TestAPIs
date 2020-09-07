using TestAPIs.Models;

namespace TestAPIs.Services
{
    public interface ISettingService
    {
        string WooliesXBaseUrl { get; }
        User WooliesXUser { get; }
    }
}