using BoDi;
using Microsoft.Extensions.Configuration;
using System;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Framework
{
    [Binding]
    public sealed class RegisterConfigurationHook
    {
        private readonly IObjectContainer _objectContainer;
        private static IConfigurationRoot _configuration;

        public RegisterConfigurationHook(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeFeature]
        public static void InitConfiguration()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsetting.json")
                .Build();
        }

        [BeforeScenario]
        public void RegisterConfiguration()
        {
            _objectContainer.RegisterInstanceAs(_configuration);
        }
    }
}
