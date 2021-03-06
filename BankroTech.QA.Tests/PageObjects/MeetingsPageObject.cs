﻿using BankroTech.QA.Framework.Attributes;
using BankroTech.QA.Framework.PageObjects;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;

namespace BankroTech.QA.Tests.PageObjects
{
    [PageName(name: "Список собраний")]
    public class MeetingsPageObject : BasePageObject
    {
        protected override string Action => "/meetings";

        [PageElement(name: "Создать собрание")]
        public IWebElement CreateMeetingBtn => WebDriver.FindElement(By.ClassName("b-floating_button"));

        public MeetingsPageObject(IWebDriver webDriver, IConfigurationRoot configuration) : base(webDriver, configuration)
        {

        }
    }
}
