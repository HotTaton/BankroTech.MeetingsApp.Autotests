using BankroTech.QA.Framework.Helpers;
using BankroTech.QA.Framework.PageObjects;
using BankroTech.QA.Framework.PageObjects.PageFactory;
using BankroTech.QA.Framework.TemplateResolver;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Framework
{
    [Binding]
    public sealed class InterfaceStepDefinition
    {
        private readonly IPageFactory _pageFactory;
        private readonly ITemplateResolverService _valueResolver;
        private readonly IWaitHelper _waitHelper;
        private readonly ScenarioContext _scenarioContext;

        public InterfaceStepDefinition(ScenarioContext scenarioContext,
                                       IPageFactory pageFactory,
                                       IWaitHelper waitHelper,
                                       ITemplateResolverService templateResolver)
        {
            _scenarioContext = scenarioContext;
            _pageFactory = pageFactory;
            _waitHelper = waitHelper;
            _valueResolver = templateResolver;            
        }

        [Given(@"я захожу на страницу ""(.*)""")]
        public void GivenЯЗахожуНаСтраницу(string pageName)
        {
            var pageObj = _pageFactory[pageName];
            pageObj.GoToPage();
            _scenarioContext.Set(pageObj, "CurrentPageObj");
        }

        [Given(@"ввожу в поле ""(.*)"" данные: ""(.*)""")]
        public void GivenВвожуВПолеДанные(string fieldName, string value)
        {
            var pageObj = _scenarioContext.Get<BasePageObject>("CurrentPageObj");
            pageObj.SetInput(fieldName, _valueResolver.Resolve(value));
        }

        [Given(@"я нажимаю на кнопку ""(.*)""")]
        [When(@"я нажимаю на кнопку ""(.*)""")]
        public void WhenЯНажимаюНаКнопку(string buttonName)
        {
            var pageObj = _scenarioContext.Get<BasePageObject>("CurrentPageObj");
            pageObj.ClickButton(buttonName);
        }

        [Then(@"я перехожу на страницу ""(.*)""")]
        public void ThenЯПерехожуНаСтраницу(string pageName)
        {
            var pageObj = _waitHelper.WaitForRedirect(pageName);
            Assert.IsNotNull(pageObj);
            _scenarioContext.Set(pageObj, "CurrentPageObj");
        }

        [Given(@"я нахожусь на странице ""(.*)""")]
        public void GivenЯНахожусьНаСтранице(string pageName)
        {
            var pageObj = _pageFactory[pageName];
            Assert.IsTrue(pageObj.IsCurrent);
            _scenarioContext.Set(pageObj, "CurrentPageObj");
        }

        [Given(@"я раскрываю панель ""(.*)""")]
        public void GivenЯРаскрываюПанель(string elemName)
        {
            var pageObj = _scenarioContext.Get<BasePageObject>("CurrentPageObj");
            pageObj.ClickOnExpansionPanel(elemName);
        }

        [Then(@"открывается новая вкладка со страницей ""(.*)""")]
        public void ThenОткрываетсяНоваяВкладкаСоСтраницей(string pageName)
        {
            var pageObj = _waitHelper.WaitForNewTab(pageName);
            Assert.IsNotNull(pageObj);
            _scenarioContext.Set(pageObj, "CurrentPageObj");
        }
    }
}
