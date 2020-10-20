using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data.Common;

namespace BankroTech.QA.Framework.SqlDriver
{
    public class PgsqlDriver : SqlDriverBase
    {
        private readonly string connectionString;

        public PgsqlDriver(IConfigurationRoot configuration)            
        {
            connectionString = configuration.GetConnectionString("OperationalConnection");
        }

        protected override DbConnection CreateConnection()
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
