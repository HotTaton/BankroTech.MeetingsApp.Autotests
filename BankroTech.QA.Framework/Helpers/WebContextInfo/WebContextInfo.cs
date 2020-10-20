using BankroTech.QA.Framework.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Titanium.Web.Proxy.Models;

namespace BankroTech.QA.Framework.Helpers
{
    //ToDo: remove static browsers info
    internal class WebContextInfo : IWebContextInfo
    {
        private class SavedContextInfo
        {
            public EventFiringWebDriver WebDriver { get; }
            public ProxyEndPoint EndPoint { get; }

            public SavedContextInfo(EventFiringWebDriver webDriver, ProxyEndPoint endPoint)
            {
                WebDriver = webDriver;
                EndPoint = endPoint;
            }
        }

        private static int currentPort = 80;
        private static readonly object locker = new object();
        private static readonly List<SavedContextInfo> freeWebContexts = new List<SavedContextInfo>();

        private readonly ProxyEndPoint _endPoint;       

        public int Port => _endPoint.Port;

        public EventFiringWebDriver WebDriver { get; }

        public WebContextInfo(IInternalContextHelper contextHelper)            
        {
            var browser = contextHelper.CurrentBrowser;
            lock (locker)
            {
                var result = freeWebContexts.FirstOrDefault(x => x.WebDriver.GetBrowser() == browser);

                if (result == null)
                {
                    WebDriver = GetWebDriver(currentPort, browser);

                    _endPoint = new TransparentProxyEndPoint(IPAddress.Loopback, currentPort, false);
                    currentPort++;
                    ProxyServerContainer.ProxyServer.AddEndPoint(_endPoint);
                }
                else
                {
                    freeWebContexts.Remove(result);

                    _endPoint = result.EndPoint;
                    WebDriver = result.WebDriver;
                }                
            }
        }

        private static EventFiringWebDriver GetWebDriver(int proxyPort, Browser browser)
        {
            var proxy = new OpenQA.Selenium.Proxy
            {
                HttpProxy = $"http://localhost:{proxyPort}",
                SslProxy = $"http://localhost:{proxyPort}",
                FtpProxy = $"http://localhost:{proxyPort}"
            };

            switch (browser)
            {
                case Browser.Chrome:
                    return new EventFiringWebDriver(CreateChrome(proxy));
                default:
                    throw new ArgumentException();
            }
        }

        private static IWebDriver CreateChrome(OpenQA.Selenium.Proxy proxy)
        {
            var options = new ChromeOptions()
            {
                Proxy = proxy
            };

            options.AddArgument("--proxy-bypass-list=<-loopback>");
            return new ChromeDriver(options);
        }

        public static void DisposeAll()
        {
            foreach (var item in freeWebContexts)
            {
                item.WebDriver.Dispose();
            }
        }

        public void Release()
        {
            lock (locker)
            {
                freeWebContexts.Add(new SavedContextInfo(WebDriver, _endPoint));
            }
        }             
    }
}
