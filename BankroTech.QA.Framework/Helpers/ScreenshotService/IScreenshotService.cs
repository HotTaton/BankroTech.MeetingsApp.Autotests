using OpenQA.Selenium;

namespace BankroTech.QA.Framework.Helpers.ScreenshotMaker
{
    internal interface IScreenshotService
    {
        void TakeScreenshot();
     
        void FlushScreenshots();
    }
}
