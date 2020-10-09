using BankroTech.QA.Framework.Attributes;
using BankroTech.QA.Framework.PageObjects;
using OpenQA.Selenium;
using System.Text.RegularExpressions;

namespace BankroTech.QA.Tests.PageObjects
{
    [PageName(name: "Текущее собрание")]
    public class CreatedMeetingPageObject : BasePageObject
    {
        protected override string Url => @"http://localhost:64507/#/meeting/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}/agenda";

        public CreatedMeetingPageObject(IWebDriver webDriver) : base(webDriver)
        {

        }

        public override bool IsCurrent => Regex.IsMatch(WebDriver.Url, Url);
    }
}
