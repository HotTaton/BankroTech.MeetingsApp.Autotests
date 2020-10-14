using BankroTech.QA.Framework.Attributes;
using BankroTech.QA.Framework.PageObjects;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;

namespace BankroTech.QA.Tests.PageObjects
{
    [PageName(name: "Создание собрания")]
    public class CreateMeetingPageObject : BasePageObject
    {
        protected override string Action => "/meeting";

        [PageElement(name: "Наименование")]
        public IWebElement Name => WebDriver.FindElement(By.CssSelector("input[name = 'name']"));

        [PageElement(name: "Дата начала")]
        public IWebElement DateBegin => WebDriver.FindElement(By.CssSelector("input[name = 'startDate_textfield_date']"));

        [PageElement(name: "Время начала")]
        public IWebElement TimeBegin => WebDriver.FindElement(By.CssSelector("input[name = 'startDate_textfield_time']"));

        [PageElement(name: "Дата окончания")]
        public IWebElement DateEnd => WebDriver.FindElement(By.CssSelector("input[name = 'endDate_textfield_date']"));

        [PageElement(name: "Время окончания")]
        public IWebElement TimeEnd => WebDriver.FindElement(By.CssSelector("input[name = 'endDate_textfield_time']"));

        [PageElement(name: "Номер дела")]
        public IWebElement CaseNumber => WebDriver.FindElement(By.CssSelector("input[name = 'CaseNumber']"));

        [PageElement(name: "Арбитражный суд")]
        public IWebElement ArbitrageCourt => WebDriver.FindElement(By.CssSelector("input[name = 'ArbitrageCourt']"));

        [PageElement(name: "Краткое наименование должника")]
        public IWebElement DebtorShortName => WebDriver.FindElement(By.CssSelector("input[name = 'DebtorShortName']"));

        [PageElement(name: "ИНН")]
        public IWebElement DebtorInn => WebDriver.FindElement(By.CssSelector("input[name = 'DebtorInn']"));

        [PageElement(name: "ОГРН")]
        public IWebElement DebtorOgrn => WebDriver.FindElement(By.CssSelector("input[name = 'DebtorOgrn']"));

        [PageElement(name: "Полное наименование должника")]
        public IWebElement DebtorFullName => WebDriver.FindElement(By.CssSelector("input[name = 'DebtorFullName']"));

        [PageElement(name: "Адрес")]
        public IWebElement DebtorAddress => WebDriver.FindElement(By.CssSelector("input[name = 'DebtorAddress']"));

        [PageElement(name: "Должник")]
        public IWebElement DebtorHeader => WebDriver.FindElement(By.CssSelector("[pane-id='debtor']"));

        [PageElement(name: "Сохранить")]
        public IWebElement SaveBtn => WebDriver.FindElement(By.ClassName("b-floating_button-el"));

        public CreateMeetingPageObject(IWebDriver webDriver, IConfigurationRoot configuration) : base(webDriver, configuration)
        {

        }
    }
}
