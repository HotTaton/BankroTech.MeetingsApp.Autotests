using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BankroTech.QA.Framework.Helpers
{
    public class CustomDataTableComparer
    {
        public void Compare(Table assertTable, List<Dictionary<string, object>> customTable)
        {
            if (!customTable.Any())
            {
                throw new ComparisonException("Custom table was empty");
            }

            var header = customTable[0].Keys;

            AssertThatTableHasAllNeededColumns(assertTable, header);

            var castedTableContent = new List<Dictionary<string, object>>();

            foreach (var dataRow in customTable)
            {
                var tableRow = new Dictionary<string, object>();

                foreach (var dataCell in dataRow)
                {
                    var columnName = FindColumnAlias(assertTable.Header, dataCell.Key);

                    if (columnName != null)
                    {
                        tableRow.Add(columnName, dataCell.Value);
                    }
                }

                castedTableContent.Add(tableRow);
            }

            AssertThatContentIsEqual(assertTable, castedTableContent);
        }

        private string FindColumnAlias(ICollection<string> aliases, string colName)
        {
            return aliases.FirstOrDefault(x => string.Equals(x, colName, StringComparison.OrdinalIgnoreCase));
        }

        private void AssertThatTableHasAllNeededColumns(Table table, ICollection<string> columns)
        {
            var missingColumns = new List<string>();

            foreach (var header in table.Header)
            {
                if (!columns.Any(colName => string.Equals(colName, header, StringComparison.OrdinalIgnoreCase)))
                {
                    missingColumns.Add(header);
                }
            }

            if (missingColumns.Count > 0)
            {
                throw new ComparisonException($@"The following fields do not exist:{Environment.NewLine}{string.Join(Environment.NewLine, missingColumns)}");
            }
        }

        private void AssertThatContentIsEqual(Table table, ICollection<Dictionary<string, object>> content)
        {
            var notFoundFromTable = table.Rows.ToList();
            var notFoundFromContent = new List<Dictionary<string, object>>();

            foreach (var contentItem in content)
            {
                var itemFound = false;

                foreach (var tableItem in notFoundFromTable)
                {
                    var equalRows = true;

                    foreach (var rowData in tableItem)
                    {
                        var comparer = Service.Instance.ValueComparers.FirstOrDefault(x => x.CanCompare(contentItem[rowData.Key]));

                        if (!comparer.Compare(rowData.Value, contentItem[rowData.Key]))
                        {
                            equalRows = false;
                            break;
                        }
                    }

                    if (equalRows)
                    {
                        notFoundFromTable.Remove(tableItem);
                        itemFound = true;
                        break;
                    }
                }

                if (!itemFound)
                {
                    notFoundFromContent.Add(contentItem);
                }
            }

            if (notFoundFromTable.Count > 0 || notFoundFromContent.Count > 0)
            {
                ThrowAnErrorDetailingWhichItemsAreMissing(table.Header, notFoundFromTable, notFoundFromContent);
            }
        }

        private void ThrowAnErrorDetailingWhichItemsAreMissing(ICollection<string> headers, ICollection<TableRow> hasNoMatchFromTable, ICollection<Dictionary<string, object>> hasNoMatchFromDb)
        {
            var errorText = new StringBuilder();

            errorText.AppendLine("Different content:");
            errorText.AppendLine($"  |{string.Join(" | ", headers)} |");

            foreach (var item in hasNoMatchFromTable)
            {
                var rowData = new StringBuilder();

                foreach (var headerItem in headers)
                {
                    rowData.Append($" {item[headerItem]} |");
                }
                errorText.AppendLine($"- |{rowData}");
            }

            foreach (var item in hasNoMatchFromDb)
            {
                var rowData = new StringBuilder();

                foreach (var headerItem in headers)
                {
                    rowData.Append($" {item[headerItem]} |");
                }
                errorText.AppendLine($"+ |{rowData}");
            }

            throw new ComparisonException(errorText.ToString());
        }
    }
}
