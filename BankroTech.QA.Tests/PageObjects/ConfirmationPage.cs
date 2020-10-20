using BankroTech.QA.Framework.Attributes;
using BankroTech.QA.Framework.Helpers;
using BankroTech.QA.Framework.PageObjects;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;

namespace BankroTech.QA.Tests.PageObjects
{
    [PageName(name: "Подтверждение")]
    public class ConfirmationPage : BasePageObject
    {
        private readonly IWaitHelper _waitHelper;
        private readonly string _authCookieName;

        public ConfirmationPage(IWebDriver webDriver, IWaitHelper waitHelper, IConfigurationRoot configuration) : base(webDriver, configuration)
        {
            _authCookieName = configuration.GetSection("AuthCookieName").Value;
            _waitHelper = waitHelper;
        }

        protected override string Action => "/confirm";

        [PageElement(name: "Код")]
        public IWebElement Phone => WebDriver.FindElement(By.CssSelector("input[name = 'code']"));

        [PageElement(name: "Подтвердить")]
        public IWebElement LoginBtn => WebDriver.FindElement(By.ClassName("b-authentication-form-button--auth"));

        //ToDo: отправить в настроечный файлик
        public bool LoginSuccessfull()
        {
            if (WebDriver.Manage().Cookies.GetCookieNamed(_authCookieName) != null)
            {
                return true;
            }
            return _waitHelper.WaitForCookie(_authCookieName) != null;
        }
    }
}
