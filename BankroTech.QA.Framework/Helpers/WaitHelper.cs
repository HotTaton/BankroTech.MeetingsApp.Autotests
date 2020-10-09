using BankroTech.QA.Framework.PageObjects;
using BankroTech.QA.Framework.PageObjects.PageFactory;
using BankroTech.QA.Framework.Proxy;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace BankroTech.QA.Framework.Helpers
{
    public class WaitHelper
    {
        private const int DEFAULT_WAIT_INTERVAL = 30;

        private static readonly string[] waitForHttpMethods = new string[] { "GET", "POST", "PUT", "DELETE" };

        private readonly IWebDriver _webDriver;
        private readonly PageFactory _pageFactory;
        private readonly IProxyHttpService _httpService;

        public WaitHelper(IWebDriver webDriver, PageFactory pageFactory, IProxyHttpService httpService)
        {
            _webDriver = webDriver;
            _pageFactory = pageFactory;
            _httpService = httpService;
        }

        public BasePageObject WaitForNewTab(string pageName)
        {
            var page = _pageFactory[pageName];
            var wait = CreateDefaultWait(_webDriver);

            if (wait.Until(driver => page.GoToParentTab()))
            {
                return page;
            }
            return null;
        }

        public BasePageObject WaitForRedirect(string pageName)
        {
            var page = _pageFactory[pageName];
            var wait = CreateDefaultWait(_webDriver);

            if (wait.Until(driver => page.IsCurrent))
            {
                return page;
            }
            return null;
        }

        public Cookie WaitForCookie(string cookieName)
        {
            var wait = CreateDefaultWait(_webDriver);
            if (wait.Until(driver => driver.Manage().Cookies.GetCookieNamed(cookieName) != null))
            {
                return _webDriver.Manage().Cookies.GetCookieNamed(cookieName);
            }
            return null;
        }

        public bool WaitUntilAllAjaxIsCompleted()
        {
            var wait = CreateDefaultWait(_webDriver);

            return wait.Until(driver =>
            {
                var requestGuids = _httpService.HttpRequestsHistory
                                        .Where(kvp =>
                                            waitForHttpMethods.Any(methodName =>
                                                string.Equals(kvp.Value.Method, methodName, StringComparison.OrdinalIgnoreCase)))
                                        .Select(kvp => kvp.Key);
                return requestGuids.All(guid => _httpService.HttpResponsesHistory.ContainsKey(guid));
            });
        }

        private static WebDriverWait CreateDefaultWait(IWebDriver webDriver)
        {
            return new WebDriverWait(webDriver, TimeSpan.FromSeconds(DEFAULT_WAIT_INTERVAL));
        }
    }
}
