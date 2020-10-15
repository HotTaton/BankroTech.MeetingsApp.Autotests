using BankroTech.QA.Framework.API;
using BankroTech.QA.Framework.Helpers;
using BankroTech.QA.Framework.TemplateResolver;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Framework
{
    [Binding]
    public sealed class APIStepDefinition
    {
        private readonly IWaitHelper _waitHelper;
        private readonly ParamResolverWrapper _paramResolver;
        private readonly IRestClientService _restClient;
        private readonly ITemplateResolverService _resolverService;

        private readonly IContextHelper _scenarioContext;

        public APIStepDefinition(IContextHelper scenarioContext,
                                 IWaitHelper waitHelper,
                                 ParamResolverWrapper paramResolver,
                                 IRestClientService restClient,
                                 ITemplateResolverService resolverService)
        {
            _scenarioContext = scenarioContext;
            _restClient = restClient;
            _paramResolver = paramResolver;
            _waitHelper = waitHelper;
            _resolverService = resolverService;
        }        

        //ToDo: сделать регулярку, которая позволит писать много параметров
        [When(@"сохраняю параметр ""(.*)"" из результата запроса ""(.*)""")]
        public void WhenСохраняюПараметрИзРезультатаЗапроса(string paramName, string requestUrl)
        {
            _waitHelper.WaitUntilAllAjaxIsCompleted();
            var paramValue = _paramResolver.Resolve(requestUrl);
            _scenarioContext.SetParameter(paramName, paramValue);                
        }

        [Given(@"посылаю запрос ""(.*)"" с телом")]
        [When(@"посылаю запрос ""(.*)"" с телом")]
        public void GivenПосылаюЗапросСТелом(string action, string jsonBody)
        {
            var resolvedUrl = _resolverService.Resolve(action);
            var resolvedBody = _resolverService.Resolve(jsonBody);
            _restClient.PostRequest(resolvedUrl, resolvedBody);
        }

        [Then(@"результат ""(.*)"" истина")]
        public void ThenРезультатИстина(string requestUrl)
        {
            _waitHelper.WaitUntilAllAjaxIsCompleted();
            var paramValue = _paramResolver.Resolve(requestUrl);
            Assert.IsTrue(bool.TryParse(paramValue, out var result) && result);
        }

    }
}
