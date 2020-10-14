using BankroTech.QA.Framework.TemplateResolver;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;

namespace BankroTech.QA.Framework.PageObjects
{
    public abstract class ParameterizedPageObject : BasePageObject
    {
        private readonly ITemplateResolverService _resolverService;
        protected abstract string ActionTemplate { get; }
        protected override string Action => _resolverService.Resolve(ActionTemplate);

        protected ParameterizedPageObject(ITemplateResolverService resolverService, IWebDriver webDriver, IConfigurationRoot configuration) : base(webDriver, configuration)
        {
            _resolverService = resolverService;
        }
    }
}
