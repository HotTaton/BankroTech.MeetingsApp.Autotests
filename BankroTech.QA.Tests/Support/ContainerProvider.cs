using Autofac;
using BankroTech.QA.Framework.Helpers;

namespace BankroTech.QA.Tests.Support
{
    public class ContainerProvider : ContainerProviderBase
    {
        public override ContainerBuilder CreateContainerAndRegisterDependencies()
        {
            var builder = base.CreateContainerAndRegisterDependencies();

            return builder;
        }
    }
}
