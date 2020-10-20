using BankroTech.QA.Framework.PageObjects;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BankroTech.QA.Framework.Helpers
{
    public class BrowserNavigationService : IBrowserNavigationService
    {
        private readonly IWebDriver _webDriver;

        public BrowserNavigationService(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public void NavigateToPage(BasePageObject page)
        {
            _webDriver.Navigate().GoToUrl(page.AbsoluteUrl);
        }

        public bool IsCurrent(BasePageObject page)
        {            
            return string.Equals(page.AbsoluteUrl, _webDriver.Url, StringComparison.OrdinalIgnoreCase);
        }

        public bool GoToParentTab(BasePageObject page)
        {
            foreach (var handler in _webDriver.WindowHandles)
            {
                _webDriver.SwitchTo().Window(handler);

                if (IsCurrent(page))
                {
                    return true;
                }
            }

            return false;
        }

        public Dictionary<string, string> GetParameters(BasePageObject page)
        {
            var result = new Dictionary<string, string>();
            var urlRegex = Regex.Replace(page.AbsoluteUrl, @"<\w+,\s+(\w+)>", MakeParamRegex, RegexOptions.Compiled);
            var matches = Regex.Matches(_webDriver.Url, urlRegex);

            foreach (Match match in matches)
            {
                var group = match.Groups[0];
                result.Add(group.Name, group.Value);
            }

            return result;
        }

        private string MakeParamRegex(Match match)
        {
            var paramName = match.Groups[0].Value;
            var result = @$"(?'{paramName}'(?:\w|[\-\.\_\~\:\%\?\#\[\]\@\!\$\'\(\)\*\+\,\;\=])+)";
            return result;
        }
    }
}
