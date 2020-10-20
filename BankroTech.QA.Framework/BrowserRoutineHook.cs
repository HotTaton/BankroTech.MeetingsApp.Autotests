using BankroTech.QA.Framework.Helpers;
using BankroTech.QA.Framework.Helpers.ScreenshotMaker;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Framework
{
    [Binding]
    internal sealed class BrowserRoutineHook
    {
        private readonly IInternalContextHelper _contextHelper;
        private readonly IScreenshotService _screenshotService;

        public BrowserRoutineHook(IInternalContextHelper contextHelper, IScreenshotService screenshotService)
        {
            _contextHelper = contextHelper;
            _screenshotService = screenshotService;
        }

        [BeforeScenario(Order = 0)]
        public void BeforeScenario()
        {
            _contextHelper.CurrentBrowser = Browser.Chrome;
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _screenshotService.FlushScreenshots();
        }
    }
}
