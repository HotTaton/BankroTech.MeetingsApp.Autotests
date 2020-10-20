using BankroTech.QA.Framework.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Events;
using System;

namespace BankroTech.QA.Framework.Extensions
{
    //ToDo remove this
    internal static class WebDriverExtensions
    {
        public static Browser GetBrowser(this IWebDriver webDriver)
        {
            var webDriverWrapper = webDriver as EventFiringWebDriver;
            IWebDriver actualWebDriver;

            if (webDriverWrapper == null)
            {
                actualWebDriver = webDriver;
            }
            else
            {
                actualWebDriver = webDriverWrapper.WrappedDriver;
            }

            switch (actualWebDriver)
            {
                case ChromeDriver _:
                    return Browser.Chrome;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
