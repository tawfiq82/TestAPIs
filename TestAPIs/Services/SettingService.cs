using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using TestAPIs.Models;

namespace TestAPIs.Services
{
    public class SettingService : ISettingService
    {
        private readonly ILogger<SettingService> _logger;
        private readonly IOptionsSnapshot<WooliesXSetting> _wooliesXSetting;

        public string WooliesXBaseUrl
        {
            get
            {
                ValidateWooliesXSetting();

                var baseUrl = _wooliesXSetting.Value.BaseUrl;

                if (string.IsNullOrEmpty(baseUrl))
                {
                    throw new Exception("WooliesX base url is not configured.");
                }

                return baseUrl;
            }
        }

        public User WooliesXUser
        {
            get
            {
                ValidateWooliesXSetting();

                var user = new User
                {
                    Name = _wooliesXSetting.Value.UserName,
                    Token = _wooliesXSetting.Value.UserToken
                };

                if (string.IsNullOrEmpty(user.Name))
                {
                    throw new Exception("User is not correclty configured for WooliesX.");
                }

                if (user.Token == null || user.Token == Guid.Empty)
                {
                    throw new Exception("User Token is not correclty configured for WooliesX.");
                }

                return user;
            }
        }

        public SettingService(IOptionsSnapshot<WooliesXSetting> wooliesXSetting, ILogger<SettingService> logger)
        {
            _wooliesXSetting = wooliesXSetting;
            _logger = logger;
        }

        private void ValidateWooliesXSetting()
        {
            if (_wooliesXSetting == null || _wooliesXSetting.Value == null)
                throw new Exception("Failed to load WooliesX Settings.");
        }
    }
}
