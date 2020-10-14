using BankroTech.QA.Framework.Attributes;
using BankroTech.QA.Framework.PageObjects;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;

namespace BankroTech.QA.Tests.PageObjects
{
    [PageName(name: "Логин")]
    public class LoginPage : BasePageObject
    {
        public LoginPage(IWebDriver webDriver, IConfigurationRoot configuration) : base(webDriver, configuration) { }

        #region UI Elements

        [PageElement(name: "Заголовок")]
        public IWebElement SiteName => WebDriver.FindElement(By.ClassName("b-authentication-form-title"));

        [PageElement(name: "Телефон")]
        public IWebElement Phone => WebDriver.FindElement(By.CssSelector("input[name = 'phone']"));

        [PageElement(name: "Пароль")]
        public IWebElement Password => WebDriver.FindElement(By.CssSelector("input[name = 'password']"));

        [PageElement(name: "Войти")]
        public IWebElement LoginBtn => WebDriver.FindElement(By.ClassName("b-authentication-form-button--auth"));

        protected override string Action => "/login";
        #endregion UI Elements

        #region Actions     
        //public bool IsWrongAuthData() => _webDriver.FindElement(By.Name("password")).GetAttribute("class").Contains("ng-invalid");

        //
        #endregion Actions
    }
}
