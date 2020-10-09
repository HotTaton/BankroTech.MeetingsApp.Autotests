using System.Collections.Generic;

namespace BankroTech.QA.Framework.SqlDriver
{
    public interface ISqlDriver
    {
        List<Dictionary<string, object>> ExecuteQuery(string query);
    }
}
