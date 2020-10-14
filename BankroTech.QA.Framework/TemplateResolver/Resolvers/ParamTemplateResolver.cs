using BankroTech.QA.Framework.Helpers;
using BankroTech.QA.Framework.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BankroTech.QA.Framework.TemplateResolver.Resolvers
{
    public class ParamTemplateResolver : ITemplateResolver
    {
        private readonly IContextHelper _scenarioContext;
        private readonly IProxyHttpService _httpService;

        public ParamTemplateResolver(IContextHelper scenarioContext, IProxyHttpService httpService)
        {
            _scenarioContext = scenarioContext;
            _httpService = httpService;
        }

        public virtual string GetData(IEnumerable<string> args)
        {
            var arg = args.Single();
            var result = TryGetData(arg);

            return result;
        }

        protected string TryGetData(string arg)
        {
            var result = string.Empty;

            result = TryGetResultFromContextVariableByName(arg);

            if (string.IsNullOrEmpty(result))
            {
                result = TryGetResultFromSqlQueryResult(arg);

                if (string.IsNullOrEmpty(result))
                {
                    result = TryGetResultFromResponseBody(arg);
                }
            }

            return result;
        }

        private string TryGetResultFromContextVariableByName(string variableName)
        {
            return _scenarioContext.GetParameter<string>(variableName) ?? string.Empty;            
        }

        private string TryGetResultFromSqlQueryResult(string arg)
        {
            (var locator, var index, var paramName) = ProcessArgument(arg);

            if (string.Equals("Результат SQL запроса", locator, StringComparison.OrdinalIgnoreCase))
            {
                if (!index.HasValue)
                {
                    index = 0;
                }

                var sqlResult = _scenarioContext.StoredData;
                return sqlResult[index.Value][paramName].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private string TryGetResultFromResponseBody(string arg)
        {
            (var action, var index, var paramName) = ProcessArgument(arg);

            var splittedValue = action.Split(' ');
            var httpMethod = splittedValue[0].ToUpper();
            var url = splittedValue[1];

            var responseBody = index.HasValue ?
                                  _httpService.GetResponseBody(httpMethod, url, index.Value) :
                                  _httpService.GetLastResponseBody(httpMethod, url);

            var pathElements = paramName.Split('.');

            var currentElem = responseBody;
            foreach (var element in pathElements)
            {
                if (currentElem[element] != null)
                {
                    currentElem = currentElem[element];
                }
                else
                {
                    return string.Empty;
                }
            }

            return currentElem.ToString();
        }

        private (string locator, int? index, string paramName) ProcessArgument(string arg)
        {
            var match = Regex.Match(arg, @"(?'Locator'.+?)(?:\[(?'Index'\d+)\])?\.(?'ParamName'.+)", RegexOptions.Compiled);

            var locator = match.Groups["Locator"].Value.Trim();
            int? index = null;
            if (!string.IsNullOrEmpty(match.Groups["Position"].Value))
            {
                index = int.Parse(match.Groups["Index"].Value);
            }
            var paramName = match.Groups["ParamName"].Value;

            return (locator, index, paramName);
        }
    }
}
