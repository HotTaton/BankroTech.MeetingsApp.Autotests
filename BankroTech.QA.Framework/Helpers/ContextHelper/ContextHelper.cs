using BankroTech.QA.Framework.PageObjects;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Framework.Helpers
{
    public class ContextHelper : IContextHelper
    {
        private const string CURRENT_PAGE_LITERAL = "CurrentPageObj";
        private const string DEFAULT_STORED_DATA_LITERAL = "DefaultSqlQueryResult";

        private const string PARAM_LITERAL = "PARAM:";

        private readonly ScenarioContext _scenarioContext;

        public ContextHelper(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        public virtual BasePageObject CurrentPage
        {
            get => _scenarioContext.Get<BasePageObject>(CURRENT_PAGE_LITERAL);
            set => _scenarioContext.Set(value, CURRENT_PAGE_LITERAL);
        }

        public virtual List<Dictionary<string, object>> StoredData
        {
            get => _scenarioContext.Get<List<Dictionary<string, object>>>(DEFAULT_STORED_DATA_LITERAL);
            set => _scenarioContext.Set(value, DEFAULT_STORED_DATA_LITERAL);
        }

        public virtual void SetParameter<T>(string paramName, T paramValue)
        {
            _scenarioContext.Set(paramValue, $"{PARAM_LITERAL}{paramName}".ToUpper());
        }

        public virtual T GetParameter<T>(string paramName)
        {
            var key = $"{PARAM_LITERAL}{paramName}".ToUpper();

            if (_scenarioContext.ContainsKey(key))
            {
                return _scenarioContext.Get<T>(key);
            }

            return default;
        }
    }
}
