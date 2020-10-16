using OpenQA.Selenium;
using System.Collections.Concurrent;
using System.IO;

namespace BankroTech.QA.Framework.Helpers.ScreenshotMaker
{
    internal class ScreenshotService : IScreenshotService
    {
        private ConcurrentDictionary<ITakesScreenshot, string> _browserAliases = new ConcurrentDictionary<ITakesScreenshot, string>();
        private ConcurrentDictionary<ITakesScreenshot, ConcurrentQueue<Screenshot>> _screenshotsContainer = new ConcurrentDictionary<ITakesScreenshot, ConcurrentQueue<Screenshot>>();

        public void ClearScreenshotCache()
        {
            foreach (var kvp in _screenshotsContainer)
            {
                kvp.Value.Clear();
            }
        }

        public void FlushScreenshots(ITakesScreenshot browser, string path)
        {
            if (!_screenshotsContainer.ContainsKey(browser))
            {
                return;
            }

            TakeScreenshot(browser);
            var browserScreenshotsCache = _screenshotsContainer[browser];

            if (!path.EndsWith('\\'))
            {
                path += "\\";
            }

            var subfolderName = _browserAliases[browser];
            var directory = Directory.CreateDirectory(path + subfolderName);

            var filePosition = 0;
            while (browserScreenshotsCache.TryDequeue(out var screenshot))
            {
                screenshot.SaveAsFile($"{directory.FullName}\\{filePosition}.png");
                filePosition++;
            }           
        }

        public void FlushAll(string path)
        {
            foreach (var kvp in _screenshotsContainer)
            {
                FlushScreenshots(kvp.Key, path);
            }
        }

        public void TakeScreenshot(ITakesScreenshot browser)
        {
            if (browser == null || !_screenshotsContainer.ContainsKey(browser))
            {
                return;
            }
            
            _screenshotsContainer[browser].Enqueue(browser.GetScreenshot());
        }

        public void RegistrateBrowser(ITakesScreenshot browser, string name)
        {
            if (!_browserAliases.ContainsKey(browser))
            {
                _screenshotsContainer.TryAdd(browser, new ConcurrentQueue<Screenshot>());
                _browserAliases.TryAdd(browser, name);
            }
        }

    }
}
