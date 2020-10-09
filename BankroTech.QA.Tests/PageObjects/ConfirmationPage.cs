using BankroTech.QA.Framework.Attributes;
using BankroTech.QA.Framework.Helpers;
using BankroTech.QA.Framework.PageObjects;
using OpenQA.Selenium;

namespace BankroTech.QA.Tests.PageObjects
{
    [PageName(name: "Подтверждение")]
    public class ConfirmationPage : BasePageObject
    {
        private readonly WaitHelper _waitHelper;

        public ConfirmationPage(IWebDriver webDriver, WaitHelper waitHelper) : base(webDriver)
        {
            _waitHelper = waitHelper;
        }

        protected override string Url => "http://localhost:64507/#/confirm";

        [PageElement(name: "Код")]
        public IWebElement Phone => WebDriver.FindElement(By.CssSelector("input[name = 'code']"));

        [PageElement(name: "Подтвердить")]
        public IWebElement LoginBtn => WebDriver.FindElement(By.ClassName("b-authentication-form-button--auth"));

        //ToDo: отправить в настроечный файлик
        public bool LoginSuccessfull()
        {
            const string authCookieName = ".AspNetCore.Cookies.64507";
            if (WebDriver.Manage().Cookies.GetCookieNamed(authCookieName) != null)
            {
                return true;
            }
            return _waitHelper.WaitForCookie(authCookieName) != null;
        }
    }
}
