using BankroTech.QA.Framework.TemplateResolver;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data.Common;

namespace BankroTech.QA.Framework.SqlDriver
{
    public class PgsqlDriver : SqlDriverBase
    {
        private readonly IConfigurationRoot _configuration;

        public PgsqlDriver(TemplateResolverService resolverService, IConfigurationRoot configuration)
            : base(resolverService)
        {
            _configuration = configuration;
        }

        protected override DbConnection CreateConnection()
        {
            return new NpgsqlConnection(_configuration.GetConnectionString("OperationalConnection"));
        }
    }
}
