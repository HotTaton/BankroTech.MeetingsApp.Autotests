using BankroTech.QA.Framework.Helpers;
using BankroTech.QA.Framework.Proxy;
using System.Collections.Generic;
using System.Linq;

namespace BankroTech.QA.Framework.TemplateResolver.Resolvers
{
    public class UrlTemplateResolver : ParamTemplateResolver
    {
        public UrlTemplateResolver(IContextHelper scenarioContext, IProxyHttpService httpService) : base(scenarioContext, httpService)
        {
        }

        public override string GetData(IEnumerable<string> args)
        {
            var arg = args.Single();
            var result = TryGetData(arg);

            return $"{arg}={result}";
        }
    }
}
