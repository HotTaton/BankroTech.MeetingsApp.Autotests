using BankroTech.QA.Framework.Attributes;
using BankroTech.QA.Framework.PageObjects;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using System.Text.RegularExpressions;

namespace BankroTech.QA.Tests.PageObjects
{
    [PageName(name: "Текущее собрание")]
    public class CreatedMeetingPageObject : BasePageObject
    {
        protected override string Url => @"/meeting/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}/agenda";

        public CreatedMeetingPageObject(IWebDriver webDriver, IConfigurationRoot configuration) : base(webDriver, configuration)
        {

        }

        public override bool IsCurrent => Regex.IsMatch(WebDriver.Url, AbsoluteUrl);
    }
}
