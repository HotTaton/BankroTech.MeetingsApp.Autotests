using BankroTech.QA.Framework.Helpers;
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
        private readonly IContextHelper _scenarioContext;
        private readonly IBrowserNavigationService _browserNavigation;

        public InterfaceStepDefinition(IContextHelper scenarioContext,
                                       IPageFactory pageFactory,
                                       IWaitHelper waitHelper,
                                       ITemplateResolverService templateResolver,
                                       IBrowserNavigationService browserNavigation)
        {
            _scenarioContext = scenarioContext;
            _pageFactory = pageFactory;
            _waitHelper = waitHelper;
            _valueResolver = templateResolver;
            _browserNavigation = browserNavigation;
        }

        [Given(@"я захожу на страницу ""(.*)""")]
        public void GivenЯЗахожуНаСтраницу(string pageName)
        {
            var pageObj = _pageFactory[pageName];
            _browserNavigation.NavigateToPage(pageObj);
            _scenarioContext.CurrentPage = pageObj;
        }

        [Given(@"ввожу в поле ""(.*)"" данные: ""(.*)""")]
        public void GivenВвожуВПолеДанные(string fieldName, string value)
        {
            var pageObj = _scenarioContext.CurrentPage;
            pageObj.SetInput(fieldName, _valueResolver.Resolve(value));
        }

        [Given(@"я нажимаю на кнопку ""(.*)""")]
        [When(@"я нажимаю на кнопку ""(.*)""")]
        public void WhenЯНажимаюНаКнопку(string buttonName)
        {
            var pageObj = _scenarioContext.CurrentPage;
            pageObj.ClickButton(buttonName);
        }

        [Then(@"я перехожу на страницу ""(.*)""")]
        public void ThenЯПерехожуНаСтраницу(string pageName)
        {
            var pageObj = _waitHelper.WaitForRedirect(pageName);
            Assert.IsNotNull(pageObj);
            _scenarioContext.CurrentPage = pageObj;
        }

        [Given(@"я нахожусь на странице ""(.*)""")]
        public void GivenЯНахожусьНаСтранице(string pageName)
        {
            var pageObj = _pageFactory[pageName];
            Assert.IsTrue(_browserNavigation.IsCurrent(pageObj));
            _scenarioContext.CurrentPage = pageObj;
        }

        [Given(@"я раскрываю панель ""(.*)""")]
        public void GivenЯРаскрываюПанель(string elemName)
        {
            var pageObj = _scenarioContext.CurrentPage;
            pageObj.ClickOnExpansionPanel(elemName);
        }

        [Then(@"открывается новая вкладка со страницей ""(.*)""")]
        public void ThenОткрываетсяНоваяВкладкаСоСтраницей(string pageName)
        {
            var pageObj = _waitHelper.WaitForNewTab(pageName);
            Assert.IsNotNull(pageObj);
            _scenarioContext.CurrentPage = pageObj;
        }
    }
}
