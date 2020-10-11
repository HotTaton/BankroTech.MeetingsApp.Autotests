using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BankroTech.QA.Framework.Helpers
{
    internal static class WebDriverContainer
    {
        private const int PROXY_PORT = 80;

        private static IWebDriver _webDriverInstance;

        public static IWebDriver WebDriver
        {
            get
            {
                if (_webDriverInstance == null)
                {
                    var options = CreateOptions();
                    _webDriverInstance = new ChromeDriver(options);
                }
                return _webDriverInstance;
            }
        }

        private static ChromeOptions CreateOptions()
        {
            var proxy = new OpenQA.Selenium.Proxy
            {
                HttpProxy = $"http://localhost:{PROXY_PORT}",
                SslProxy = $"http://localhost:{PROXY_PORT}",
                FtpProxy = $"http://localhost:{PROXY_PORT}"
            };

            var options = new ChromeOptions()
            {
                Proxy = proxy
            };

            options.AddArgument("--proxy-bypass-list=<-loopback>");

            return options;
        }
    }
}
