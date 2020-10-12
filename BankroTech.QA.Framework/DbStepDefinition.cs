using BankroTech.QA.Framework.Helpers;
using BankroTech.QA.Framework.SqlDriver;
using BankroTech.QA.Framework.TemplateResolver;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Framework
{
    [Binding]
    public sealed class DbStepDefinition
    {        
        private readonly ISqlDriver _sqlQueryService;
        private readonly IContextHelper _scenarioContext;

        public DbStepDefinition(IContextHelper scenarioContext,
                                ITemplateResolverService templateResolver,
                                ISqlDriver sqlQueryService)
        {
            _scenarioContext = scenarioContext;            
            _sqlQueryService = sqlQueryService;                   
        }

        [Given(@"выполняю запрос")]
        [Then(@"выполняю запрос")]
        public void ThenВыполняюЗапрос(string sqlRequest)
        {
            var queryResult = _sqlQueryService.ExecuteQuery(sqlRequest);
            _scenarioContext.StoredData = queryResult;                
        }
    }
}
