using Autofac;
using BankroTech.QA.Framework.Helpers;
using Microsoft.Extensions.Configuration;
using System;

namespace BankroTech.QA.Tests.Support
{
    public class ContainerProvider : ContainerProviderBase
    {
        public override ContainerBuilder CreateContainerAndRegisterDependencies()
        {
            var builder = base.CreateContainerAndRegisterDependencies();

            var configuration = InitConfiguration();
            builder.RegisterInstance(configuration);

            return builder;
        }

        private IConfigurationRoot InitConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                        .AddJsonFile("appsetting.json")
                                        .Build();
            return configuration;
        }
    }
}
