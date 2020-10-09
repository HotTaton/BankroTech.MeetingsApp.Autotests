using BankroTech.QA.Framework.TemplateResolver;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace BankroTech.QA.Framework.SqlDriver
{
    public abstract class SqlDriverBase : ISqlDriver
    {
        private readonly TemplateResolverService _resolverService;

        public SqlDriverBase(TemplateResolverService resolverService)
        {
            _resolverService = resolverService;
        }

        protected abstract DbConnection CreateConnection();

        public List<Dictionary<string, object>> ExecuteQuery(string query)
        {
            var resolvedQuery = _resolverService.Resolve(query);

            using (var sqlConnection = CreateConnection())
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = resolvedQuery;

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            throw new Exception("SQL table was empty");
                        }

                        var dbColumns = new List<string>();

                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            dbColumns.Add(reader.GetName(i));
                        }

                        var tableContent = new List<Dictionary<string, object>>();

                        do
                        {
                            var tableRow = new Dictionary<string, object>();

                            for (var i = 0; i < reader.FieldCount; i++)
                            {
                                tableRow.Add(reader.GetName(i), reader[i]);
                            }

                            tableContent.Add(tableRow);
                        } while (reader.Read());

                        return tableContent;
                    }
                }
            }
        }
    }
}
