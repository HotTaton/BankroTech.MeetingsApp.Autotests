using BankroTech.QA.Framework.TemplateResolver;

namespace BankroTech.QA.Framework.Helpers
{
    public sealed class ParamResolverWrapper
    {
        private readonly ITemplateResolverService _resolverService;

        public ParamResolverWrapper(ITemplateResolverService resolverService)
        {
            _resolverService = resolverService;
        }

        public string Resolve(string param) => _resolverService.Resolve($"<Параметр, {param}>");
    }
}
