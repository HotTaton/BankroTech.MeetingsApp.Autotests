namespace BankroTech.QA.Framework.PageObjects.PageFactory
{
    public interface IPageFactory
    {
        BasePageObject this[string pageObjectName] { get; }

        T Get<T>(string pageObjectName);
    }
}