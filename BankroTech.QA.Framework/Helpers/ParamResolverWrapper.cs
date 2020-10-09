using BankroTech.QA.Framework.TemplateResolver;

namespace BankroTech.QA.Framework.Helpers
{
    public sealed class ParamResolverWrapper
    {
        private readonly TemplateResolverService _resolverService;

        public ParamResolverWrapper(TemplateResolverService resolverService)
        {
            _resolverService = resolverService;
        }

        public string Resolve(string param) => _resolverService.Resolve($"<Параметр, {param}>");
    }
}
