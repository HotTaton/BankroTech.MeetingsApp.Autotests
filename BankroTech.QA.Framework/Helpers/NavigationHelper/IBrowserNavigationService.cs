using BankroTech.QA.Framework.PageObjects;
using System.Collections.Generic;

namespace BankroTech.QA.Framework.Helpers
{
    public interface IBrowserNavigationService
    {
        Dictionary<string, string> GetParameters(BasePageObject page);
        bool GoToParentTab(BasePageObject page);
        bool GoToParentTab(BasePageObject page, string parameters);
        bool IsCurrent(BasePageObject page);
        bool IsCurrent(BasePageObject page, string parameters);
        void NavigateToPage(BasePageObject page);
        void NavigateToPage(BasePageObject page, string parameters);
    }
}