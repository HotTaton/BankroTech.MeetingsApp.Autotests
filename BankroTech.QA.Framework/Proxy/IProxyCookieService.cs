namespace BankroTech.QA.Framework.Proxy
{
    public interface IProxyCookieService
    {
        void SetCookie(string key, string value);
        void ClearCookies();
    }
}
