using BankroTech.QA.Framework.SqlDriver;
using BankroTech.QA.Framework.TemplateResolver;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Framework
{
    [Binding]
    public sealed class DbStepDefinition
    {
        private readonly ITemplateResolverService _valueResolver;
        private readonly ISqlDriver _sqlQueryService;

        private readonly ScenarioContext _scenarioContext;

        public DbStepDefinition(ScenarioContext scenarioContext,
                                ITemplateResolverService templateResolver,
                                ISqlDriver sqlQueryService)
        {
            _scenarioContext = scenarioContext;            
            _sqlQueryService = sqlQueryService; 
            _valueResolver = templateResolver;            
        }

        [When(@"я подставляю параметр в запрос")]
        public void WhenЯПодставляюПараметрВЗапрос(string sqlRequest)
        {
            var resolvedRequest = _valueResolver.Resolve(sqlRequest);
            _scenarioContext.Add("SqlRequest", resolvedRequest);
        }

        [Then(@"выполняю запрос")]
        public void ThenВыполняюЗапрос(string sqlRequest)
        {
            var queryResult = _sqlQueryService.ExecuteQuery(sqlRequest);
            _scenarioContext.Add("SqlQueryResult", queryResult);
        }
    }
}
