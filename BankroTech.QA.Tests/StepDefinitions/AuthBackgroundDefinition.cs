using BankroTech.QA.Framework.Helpers;
using BankroTech.QA.Framework.PageObjects.PageFactory;
using BankroTech.QA.Tests.PageObjects;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Tests.StepDefinitions
{
    [Binding]
    public class AuthDefinition
    {
        private readonly FeatureContext _featureContext;
        private readonly PageFactory _pageFactory;
        private readonly WaitHelper _waitHelper;

        public AuthDefinition(FeatureContext featureContext, PageFactory pageFactory, WaitHelper waitHelper)
        {
            _featureContext = featureContext;
            _pageFactory = pageFactory;
            _waitHelper = waitHelper;
        }

        [Given(@"я авторизованный пользователь")]
        public void GivenАвторизованныйПользователь()
        {
            if (_featureContext.ContainsKey("Auth"))
                return;

            var pageObj = _pageFactory["Логин"];
            pageObj.GoToPage();
            pageObj.SetInput("Телефон", "9171864323");
            pageObj.SetInput("Пароль", "12345678");
            pageObj.ClickButton("Войти");
            Assert.IsNotNull(_waitHelper.WaitForRedirect("Подтверждение"));

            var confirmationPage = _pageFactory["Подтверждение"] as ConfirmationPage;
            confirmationPage.ClickButton("Подтвердить");
            Assert.IsTrue(confirmationPage.LoginSuccessfull());
            _featureContext.Add("Auth", true);
        }
    }
}
