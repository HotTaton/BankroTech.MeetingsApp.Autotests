using BankroTech.QA.Framework.Helpers;
using BankroTech.QA.Framework.Helpers.ScreenshotMaker;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Framework
{
    [Binding]
    internal sealed class ReleaseWebContextHook
    {
        private readonly IWebContextInfo webContextInfo;

        public ReleaseWebContextHook(IWebContextInfo webContextInfo)
        {
            this.webContextInfo = webContextInfo;
        }

        [AfterScenario]
        public void AfterScenario()
        {            
            webContextInfo.Release();
        }
    }
}
