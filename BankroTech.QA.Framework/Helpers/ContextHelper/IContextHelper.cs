using BankroTech.QA.Framework.PageObjects;
using System.Collections.Generic;

namespace BankroTech.QA.Framework.Helpers
{
    public interface IContextHelper
    {
        BasePageObject CurrentPage { get; set; }
        List<Dictionary<string, object>> StoredData { get; set; }

        T GetParameter<T>(string paramName);
        void SetParameter<T>(string paramName, T paramValue);
    }
}