using OpenQA.Selenium;

namespace BankroTech.QA.Framework.Helpers.ScreenshotMaker
{
    internal interface IScreenshotService
    {
        void RegistrateBrowser(ITakesScreenshot browser, string name);
        void TakeScreenshot(ITakesScreenshot browser);
        void ClearScreenshotCache();
        void FlushScreenshots(ITakesScreenshot browser, string path);
        void FlushAll(string path);
    }
}
