using BankroTech.QA.Framework.Helpers;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Framework
{
    [Binding]
    public sealed class HeavyServicesHook
    {
        [BeforeTestRun]
        public static void InitHeavyServices()
        {
            ProxyServerContainer.ProxyServer.Start();            
        }        

        [AfterTestRun]
        public static void ReleaseHeavyServices()
        {
            WebDriverContainer.WebDriver.Dispose();
            ProxyServerContainer.ProxyServer.Stop();
        }        
    }
}
