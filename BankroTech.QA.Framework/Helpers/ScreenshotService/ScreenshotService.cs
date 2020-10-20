using BankroTech.QA.Framework.Extensions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Framework.Helpers.ScreenshotMaker
{
    internal class ScreenshotService : IScreenshotService, IDisposable
    {        
        private static readonly ConcurrentDictionary<ITakesScreenshot, ConcurrentQueue<Screenshot>> screenshotsContainer = new ConcurrentDictionary<ITakesScreenshot, ConcurrentQueue<Screenshot>>();

        private readonly string _screenshotsPath;
        private readonly string _featureTitle;
        private readonly EventFiringWebDriver _webDriver;

        public ScreenshotService(IConfigurationRoot configuration, FeatureContext currentFeature, EventFiringWebDriver webDriver)
        {
            _screenshotsPath = configuration.GetSection("ScreenshotsLocation").Value;
            _featureTitle = currentFeature.FeatureInfo.Title;
            _webDriver = webDriver;
            screenshotsContainer.TryAdd(webDriver, new ConcurrentQueue<Screenshot>());

            _webDriver.ElementClicked += new EventHandler<WebElementEventArgs>(TakeScreenshotListener);
            _webDriver.Navigated += new EventHandler<WebDriverNavigationEventArgs>(TakeScreenshotListener);
            _webDriver.ElementValueChanged += new EventHandler<WebElementValueEventArgs>(TakeScreenshotListener);
            _webDriver.ExceptionThrown += new EventHandler<WebDriverExceptionEventArgs>(TakeScreenshotListener);
        }

        public void Dispose()
        {
            screenshotsContainer.TryRemove(_webDriver, out _);

            _webDriver.ElementClicked -= new EventHandler<WebElementEventArgs>(TakeScreenshotListener);
            _webDriver.Navigated -= new EventHandler<WebDriverNavigationEventArgs>(TakeScreenshotListener);
            _webDriver.ElementValueChanged -= new EventHandler<WebElementValueEventArgs>(TakeScreenshotListener);
            _webDriver.ExceptionThrown -= new EventHandler<WebDriverExceptionEventArgs>(TakeScreenshotListener);
        }

        public void FlushScreenshots()
        {
            TakeScreenshot();
            var browserScreenshotsCache = screenshotsContainer[_webDriver];
       
            var invalidFileNameChars = Path.GetInvalidFileNameChars();
            var currentTestName = TestContext.CurrentContext.Test.Name;
            var sanitazedTestName = new string(currentTestName.Where(x => !invalidFileNameChars.Contains(x)).ToArray());
            var sanitazedFeatureTitle = new string(_featureTitle.Where(x => !invalidFileNameChars.Contains(x)).ToArray());
                        
            var directory = Directory.CreateDirectory(Path.Combine(_screenshotsPath, sanitazedFeatureTitle, sanitazedTestName, _webDriver.GetBrowser().ToString()));

            var filePosition = 0;
            while (browserScreenshotsCache.TryDequeue(out var screenshot))
            {
                screenshot.SaveAsFile(Path.Combine(directory.FullName, $"{filePosition}.png"));
                filePosition++;
            }           
        }

        public void TakeScreenshot()
        {
            screenshotsContainer[_webDriver].Enqueue(_webDriver.GetScreenshot());
        }

        private void TakeScreenshotListener<T>(object sender, T eventArgs)
        {
            TakeScreenshot();
        }
    }
}
