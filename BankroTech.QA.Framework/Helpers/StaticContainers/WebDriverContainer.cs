using BankroTech.QA.Framework.Helpers.ScreenshotMaker;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Events;
using System;

namespace BankroTech.QA.Framework.Helpers
{
    internal static class WebDriverContainer
    {
        private static EventFiringWebDriver _webDriverInstance;
        private static IScreenshotService _screenshotService;

        public static IScreenshotService ScreenshotService
        {
            get
            {
                if (_screenshotService == null)
                {
                    _screenshotService = new ScreenshotService();
                }
                return _screenshotService;
            }
        }

        public static IWebDriver WebDriver
        {
            get
            {
                if (_webDriverInstance == null)
                {
                    var options = CreateOptions();
                    var browser = new ChromeDriver(options);
                    _webDriverInstance = new EventFiringWebDriver(browser);

                    ScreenshotService.RegistrateBrowser(_webDriverInstance, nameof(ChromeDriver));
                    _webDriverInstance.ElementClicked += new EventHandler<WebElementEventArgs>(TakeScreenshot);
                    _webDriverInstance.Navigated += new EventHandler<WebDriverNavigationEventArgs>(TakeScreenshot);
                    _webDriverInstance.ElementValueChanged += new EventHandler<WebElementValueEventArgs>(TakeScreenshot);
                    _webDriverInstance.ExceptionThrown += new EventHandler<WebDriverExceptionEventArgs>(TakeScreenshot);
                }
                return _webDriverInstance;
            }
        }

        private static ChromeOptions CreateOptions()
        {
            var proxyPort = ConfigurationContainer.GetProxyPort();

            var proxy = new OpenQA.Selenium.Proxy
            {
                HttpProxy = $"http://localhost:{proxyPort}",
                SslProxy = $"http://localhost:{proxyPort}",
                FtpProxy = $"http://localhost:{proxyPort}"
            };

            var options = new ChromeOptions()
            {
                Proxy = proxy
            }; 

            options.AddArgument("--proxy-bypass-list=<-loopback>");
            //options.AddArgument("--auto-open-devtools-for-tabs");

            return options;
        }

        private static void TakeScreenshot<T>(object o, T e)
        {
            ScreenshotService.TakeScreenshot(o as ITakesScreenshot);
        }
    }
}
