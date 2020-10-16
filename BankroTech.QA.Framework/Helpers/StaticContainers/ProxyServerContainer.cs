using System.Net;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.Models;
using Titanium.Web.Proxy.Network;

namespace BankroTech.QA.Framework.Helpers
{
    internal static class ProxyServerContainer
    {        
        private static ProxyServer _proxyServer;

        public static ProxyServer ProxyServer
        {
            get
            {
                if (_proxyServer == null)
                {
                    _proxyServer = new ProxyServer();

                    InstallProxyCertificate();
                    InitProxyEndpoints();

                    _proxyServer.BeforeRequest += ProxyServiceContainer.ProxyHandler.OnRequest;
                    _proxyServer.BeforeResponse += ProxyServiceContainer.ProxyHandler.OnResponse;
                }

                return _proxyServer;
            }
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
                _proxyServer.CertificateManager.CertificateEngine = CertificateEngine.BouncyCastle;
            }
        }

        private static void InitProxyEndpoints()
        {
            var proxyPort = ConfigurationContainer.GetProxyPort();
            var endpoint = new TransparentProxyEndPoint(IPAddress.Loopback, proxyPort, false);
            _proxyServer.AddEndPoint(endpoint);
        }
    }
}
