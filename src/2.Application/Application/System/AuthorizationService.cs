using Application.System.Contracts;
using Crosscutting.AppSettings;

namespace Application.System
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly AppSettings _appSettings;
        private HashSet<string> _apiKeys;

        public AuthorizationService(AppSettings appSettings)
        {
            _appSettings = appSettings;
            _apiKeys = new HashSet<string>();
            SetupValidApiKeys();
        }

        private void SetupValidApiKeys()
        {
            if (_appSettings.ApiKey != null)
            {
                var apiKeySplit = _appSettings.ApiKey.Split(',');
                foreach (var apiKey in apiKeySplit)
                    if (!_apiKeys.Contains(apiKey))
                        _apiKeys.Add(apiKey);
            }
        }

        public bool IsAuthorized(string apiKey)
        {
            if (_apiKeys.Contains(apiKey))
                return true;

            return false;
        }
    }
}
