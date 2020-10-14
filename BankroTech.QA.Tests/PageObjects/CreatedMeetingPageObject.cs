using BankroTech.QA.Framework.Attributes;
using BankroTech.QA.Framework.PageObjects;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;

namespace BankroTech.QA.Tests.PageObjects
{
    [PageName(name: "Текущее собрание")]
    public class CreatedMeetingPageObject : BasePageObject
    {
        protected override string Url => @"/meeting/<MeetingId>/agenda";

        public CreatedMeetingPageObject(IWebDriver webDriver, IConfigurationRoot configuration) : base(webDriver, configuration)
        {

        }
    }
}
