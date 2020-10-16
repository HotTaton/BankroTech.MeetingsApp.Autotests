using Microsoft.Extensions.Configuration;
using System;

namespace BankroTech.QA.Framework.Helpers
{
    internal static class ConfigurationContainer
    {
        private const int DEFAULT_PROXY_PORT = 80;
        private const string SCREENSHOTS_DEFAULT_LOCATION = "\\Screenshots";

        private static IConfigurationRoot _appConfiguration;

        public static IConfigurationRoot Configuration
        {
            get
            {
                if (_appConfiguration == null)
                {
                    _appConfiguration = new ConfigurationBuilder()
                                            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                            .AddJsonFile("appsetting.json")
                                            .Build();
                }
                return _appConfiguration;
            }
        }

        public static int GetProxyPort()
        {
            var proxyPortStr = Configuration.GetSection("ProxyPort").Value;

            if (!int.TryParse(proxyPortStr, out var proxyPort))
            {
                proxyPort = DEFAULT_PROXY_PORT;
            }

            return proxyPort;
        }

        public static string GetScreenshotsPath()
        {
            var screenshotsLocation = Configuration.GetSection("ScreenshotsLocation").Value;

            if (string.IsNullOrEmpty(screenshotsLocation))
            {
                screenshotsLocation = SCREENSHOTS_DEFAULT_LOCATION;
            }

            return screenshotsLocation;
        }
    }
}
