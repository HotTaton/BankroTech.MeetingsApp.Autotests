using BankroTech.QA.Framework.Helpers;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Framework
{
    [Binding]
    public sealed class APIStepDefinition
    {
        private readonly IWaitHelper _waitHelper;
        private readonly ParamResolverWrapper _paramResolver;

        private readonly IContextHelper _scenarioContext;

        public APIStepDefinition(IContextHelper scenarioContext,
                                 IWaitHelper waitHelper,
                                 ParamResolverWrapper paramResolver)
        {
            _scenarioContext = scenarioContext;
            _paramResolver = paramResolver;
            _waitHelper = waitHelper;            
        }        

        //ToDo: сделать регулярку, которая позволит писать много параметров
        [When(@"сохраняю параметр ""(.*)"" из результата запроса ""(.*)""")]
        public void WhenСохраняюПараметрИзРезультатаЗапроса(string paramName, string requestUrl)
        {
            _waitHelper.WaitUntilAllAjaxIsCompleted();
            var paramValue = _paramResolver.Resolve(requestUrl);
            _scenarioContext.SetParameter(paramName, paramValue);                
        }
    }
}
