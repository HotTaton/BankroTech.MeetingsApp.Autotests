using BankroTech.QA.Framework.Proxy;
using BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Linq;
using System.Net;
using TechTalk.SpecFlow;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.Models;
using Titanium.Web.Proxy.Network;
using BrowserProxy = OpenQA.Selenium.Proxy;

namespace BankroTech.QA.Framework
{
    [Binding]
    public sealed class WebDriverSupport
    {
        private const int PROXY_PORT = 80;

        private readonly IObjectContainer _objectContainer;

        private static ProxyServer _proxyServer;
        private static IWebDriver _webDriver;
        private static ProxyHandlerService _proxyHandler;

        public WebDriverSupport(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        private static void SetUpProxyEndpoint()
        {
            var explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Loopback, PROXY_PORT, false);
            _proxyServer.AddEndPoint(explicitEndPoint);
        }

        private static void InstallProxyCertificate()
        {
            _proxyServer.CertificateManager.SaveFakeCertificates = true;
            try
            {
                _proxyServer.CertificateManager.TrustRootCertificate(true);
            }
            catch
            {
                _proxyServer.CertificateManager.CertificateEngine = CertificateEngine.DefaultWindows;
                _proxyServer.CertificateManager.EnsureRootCertificate();
            }
        }

        private static void SetUpSystemHttpProxy()
        {
            var endpoint = (ExplicitProxyEndPoint)_proxyServer.ProxyEndPoints.FirstOrDefault(x => x is ExplicitProxyEndPoint);

            if (endpoint != null)
            {
                _proxyServer.SetAsSystemHttpProxy(endpoint);
                _proxyServer.SetAsSystemHttpsProxy(endpoint);
            }
        }

        private static ChromeOptions CreateOptions()
        {
            var proxy = new BrowserProxy
            {
                HttpProxy = $"http://localhost:{PROXY_PORT}",
                SslProxy = $"http://localhost:{PROXY_PORT}",
                FtpProxy = $"http://localhost:{PROXY_PORT}"
            };

            var options = new ChromeOptions()
            {
                Proxy = proxy
            };

            options.AddArgument("--proxy-bypass-list=<-loopback>");

            return options;
        }

        [BeforeFeature("Интерфейс")]
        public static void InitializeWebDriver()
        {
            _proxyServer = new ProxyServer();

            InstallProxyCertificate();

            var endpoint = new TransparentProxyEndPoint(IPAddress.Loopback, PROXY_PORT, false);
            _proxyServer.AddEndPoint(endpoint);

            _proxyServer.Start();

            _proxyHandler = new ProxyHandlerService();

            _proxyServer.BeforeRequest += _proxyHandler.OnRequest;
            _proxyServer.BeforeResponse += _proxyHandler.OnResponse;

            var options = CreateOptions();
            _webDriver = new ChromeDriver(options);
        }

        [BeforeScenario("Интерфейс")]
        public void RegisterWebDriver()
        {
            _objectContainer.RegisterInstanceAs<IWebDriver>(_webDriver);
            _objectContainer.RegisterInstanceAs<IProxyHttpService>(_proxyHandler);
        }

        [AfterFeature("Интерфейс")]
        public static void ReleaseWebDriver()
        {
            _webDriver.Dispose();
            _proxyServer.Stop();
        }
    }
}
