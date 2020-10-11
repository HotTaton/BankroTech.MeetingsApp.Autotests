using Autofac;
using SpecFlow.Autofac;

namespace BankroTech.QA.Tests.Support
{
    public static class Dependencies
    {
        [ScenarioDependencies]
        public static ContainerBuilder PrepareBuilder()
        {
            var containerProvider = new ContainerProvider();
            var builder = containerProvider.CreateContainerAndRegisterDependencies();
            return builder;
        }
    }
}
