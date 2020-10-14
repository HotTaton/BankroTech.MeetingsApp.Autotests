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
            ThrowExceptionIfParameterizedUrl(page.AbsoluteUrl);
            NavigateToPage(page, string.Empty);
        }

        public void NavigateToPage(BasePageObject page, string parameters)
        {
            var parameterizedUrl = SetParameters(page.AbsoluteUrl, parameters);
            _webDriver.Navigate().GoToUrl(parameterizedUrl);
        }

        public bool IsCurrent(BasePageObject page)
        {
            ThrowExceptionIfParameterizedUrl(page.AbsoluteUrl);
            return IsCurrent(page, string.Empty);
        }

        public bool IsCurrent(BasePageObject page, string parameters)
        {
            var parameterizedUrl = SetParameters(page.AbsoluteUrl, parameters);
            return string.Equals(parameterizedUrl, _webDriver.Url, StringComparison.OrdinalIgnoreCase);
        }

        public bool GoToParentTab(BasePageObject page)
        {
            ThrowExceptionIfParameterizedUrl(page.AbsoluteUrl);
            return GoToParentTab(page, string.Empty);
        }

        public bool GoToParentTab(BasePageObject page, string parameters)
        {
            foreach (var handler in _webDriver.WindowHandles)
            {
                _webDriver.SwitchTo().Window(handler);

                if (IsCurrent(page, parameters))
                {
                    return true;
                }
            }

            return false;
        }

        public Dictionary<string, string> GetParameters(BasePageObject page)
        {
            var result = new Dictionary<string, string>();
            var urlRegex = Regex.Replace(page.AbsoluteUrl, @"(<\w+>)", MakeParamRegex, RegexOptions.Compiled);
            var matches = Regex.Match(_webDriver.Url, urlRegex);

            foreach (Group group in matches.Groups)
            {
                result.Add(group.Name, group.Value);
            }

            return result;
        }

        private string SetParameters(string pageUrl, string parameters)
        {
            var result = pageUrl;

            if (!string.IsNullOrEmpty(parameters))
            {
                var keyValuePairs = parameters.Split('&');

                foreach (var kvp in keyValuePairs)
                {
                    var splittedKvp = kvp.Split('=');
                    var key = splittedKvp[0];
                    var val = splittedKvp[1];

                    if (result.Contains('?') && result.IndexOf('?') < result.IndexOf($"<{key}>", StringComparison.OrdinalIgnoreCase))
                    {
                        result = result.Replace($"<{key}>", kvp, StringComparison.OrdinalIgnoreCase);
                    }
                    else
                    {
                        result = result.Replace($"<{key}>", val, StringComparison.OrdinalIgnoreCase);
                    }
                }
            }

            return result;
        }

        private string MakeParamRegex(Match match)
        {
            var paramName = match.Value.Trim(new char[] { '<', '>' });
            var result = @$"(?'{paramName}'(?:\w|[\-\.\_\~\:\%\?\#\[\]\@\!\$\'\(\)\*\+\,\;\=])+)";
            return result;
        }

        private void ThrowExceptionIfParameterizedUrl(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new Exception($"You should add parameters to the {url} first!");
            }
        }

    }
}
