using BankroTech.QA.Framework.Attributes;
using BankroTech.QA.Framework.PageObjects;
using BankroTech.QA.Framework.TemplateResolver;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;

namespace BankroTech.QA.Tests.PageObjects
{
    [PageName(name: "Текущее собрание")]
    public class CreatedMeetingPageObject : ParameterizedPageObject
    {
        protected override string ActionTemplate => @"/meeting/<Параметр, Id>/agenda";

        public CreatedMeetingPageObject(ITemplateResolverService templateResolver,
                                        IWebDriver webDriver,
                                        IConfigurationRoot configuration) : base(templateResolver, webDriver, configuration)
        {

        }
    }
}
