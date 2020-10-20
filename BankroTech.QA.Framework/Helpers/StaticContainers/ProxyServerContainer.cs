using Titanium.Web.Proxy;
using Titanium.Web.Proxy.Network;

namespace BankroTech.QA.Framework.Helpers
{
    internal static class ProxyServerContainer
    {
        private readonly static object locker = new object();
        private static ProxyServer _proxyServer;

        public static ProxyServer ProxyServer
        {
            get
            {
                if (_proxyServer == null)
                {
                    lock (locker)
                    {
                        if (_proxyServer == null)
                        {
                            _proxyServer = new ProxyServer();

                            InstallProxyCertificate();
                        }                        
                    }
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
    }
}
