using BankroTech.QA.Framework.Proxy;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Framework.TemplateResolver.Resolvers
{
    public class ParamTemplateResolver : ITemplateResolver
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IProxyHttpService _httpService;

        public ParamTemplateResolver(ScenarioContext scenarioContext, IProxyHttpService httpService)
        {
            _scenarioContext = scenarioContext;
            _httpService = httpService;
        }

        public string GetData(IEnumerable<string> args)
        {
            var arg = args.Single();

            var result = string.Empty;

            result = TryGetResultFromContextVariableByName(arg);

            if (string.IsNullOrEmpty(result))
            {
                result = TryGetResultFromResponseBody(arg);
            }

            return result;
        }

        private string TryGetResultFromContextVariableByName(string variableName)
        {
            var paramName = $"Param:{variableName}".ToUpper();
            if (_scenarioContext.ContainsKey(paramName))
            {
                return _scenarioContext.Get<string>(paramName);
            }
            return string.Empty;
        }

        private string TryGetResultFromResponseBody(string arg)
        {
            (var url, var index, var paramName) = ProcessArgument(arg);

            var responseBody = index.HasValue ?
                                  _httpService.GetResponseBody(url, index.Value) :
                                  _httpService.GetLastResponseBody(url);

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

        private (string url, int? index, string paramName) ProcessArgument(string arg)
        {
            var match = Regex.Match(arg, @"(?'Url'.+?)(?:\[(?'Index'\d+)\])?\.(?'ParamName'.+)", RegexOptions.Compiled);

            var url = match.Groups["Url"].Value;
            int? index = null;
            if (!string.IsNullOrEmpty(match.Groups["Position"].Value))
            {
                index = int.Parse(match.Groups["Index"].Value);
            }
            var paramName = match.Groups["ParamName"].Value;

            return (url, index, paramName);
        }
    }
}
