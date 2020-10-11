using BankroTech.QA.Framework.PageObjects;
using OpenQA.Selenium;

namespace BankroTech.QA.Framework.Helpers
{
    public interface IWaitHelper
    {
        Cookie WaitForCookie(string cookieName);
        BasePageObject WaitForNewTab(string pageName);
        BasePageObject WaitForRedirect(string pageName);
        bool WaitUntilAllAjaxIsCompleted();
    }
}