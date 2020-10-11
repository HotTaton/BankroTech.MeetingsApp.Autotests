using BankroTech.QA.Framework.Proxy;

namespace BankroTech.QA.Framework.Helpers
{
    internal static class ProxyServiceContainer
    {
        private static ProxyHandlerService _proxyHandler;

        public static ProxyHandlerService ProxyHandler
        {
            get
            {
                if (_proxyHandler == null)
                {
                    _proxyHandler = new ProxyHandlerService();
                }
                return _proxyHandler;
            }
        }
    }
}
