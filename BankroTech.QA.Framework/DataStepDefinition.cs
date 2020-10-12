using BankroTech.QA.Framework.Extensions;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Framework
{
    [Binding]
    public sealed class DataStepDefinition
    {
        private readonly ScenarioContext _scenarioContext;

        public DataStepDefinition(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Then(@"вижу следующие данные")]
        public void ThenВижуСледующиеДанные(Table table)
        {
            var sqlQueryResult = _scenarioContext.Get<List<Dictionary<string, object>>>("SqlQueryResult");
            table.CompareToCustomTable(sqlQueryResult);
        }
    }
}
