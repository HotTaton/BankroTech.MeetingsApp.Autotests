using OpenQA.Selenium.Support.Events;

namespace BankroTech.QA.Framework.Helpers
{
    internal interface IWebContextInfo
    {
        int Port { get; }
        EventFiringWebDriver WebDriver { get; }

        void Release();
    }
}