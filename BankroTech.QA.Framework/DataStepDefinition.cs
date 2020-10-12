using BankroTech.QA.Framework.Extensions;
using BankroTech.QA.Framework.Helpers;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Framework
{
    [Binding]
    public sealed class DataStepDefinition
    {
        private readonly IContextHelper _scenarioContext;

        public DataStepDefinition(IContextHelper scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Then(@"вижу следующие данные")]
        public void ThenВижуСледующиеДанные(Table table)
        {
            var sqlQueryResult = _scenarioContext.StoredData;
            table.CompareToCustomTable(sqlQueryResult);
        }
    }
}
