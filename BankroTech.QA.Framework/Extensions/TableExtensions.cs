using BankroTech.QA.Framework.Helpers;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Framework.Extensions
{
    public static class TableExtensions
    {
        public static void CompareToCustomTable(this Table table, List<Dictionary<string, object>> customTable)
        {
            var tableComparer = new CustomDataTableComparer();
            tableComparer.Compare(table, customTable);
        }
    }
}
