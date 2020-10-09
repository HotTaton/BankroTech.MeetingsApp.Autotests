using BankroTech.QA.Framework.Extensions;
using BankroTech.QA.Framework.Helpers;
using BankroTech.QA.Framework.PageObjects;
using BankroTech.QA.Framework.PageObjects.PageFactory;
using BankroTech.QA.Framework.Proxy;
using BankroTech.QA.Framework.SqlDriver;
using BankroTech.QA.Framework.TemplateResolver;
using NUnit.Framework;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Framework
{
    [Binding]
    public sealed class LoginDefinition
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly PageFactory _pageFactory;
        private readonly WaitHelper _waitHelper;
        private readonly TemplateResolverService _valueResolver;
        private readonly IProxyHttpService _httpService;
        private readonly ParamResolverWrapper _paramResolver;
        private readonly ISqlDriver _sqlQueryService;

        public LoginDefinition(ScenarioContext scenarioContext,
                               PageFactory pageFactory,
                               WaitHelper waitHelper,
                               TemplateResolverService templateResolver,
                               IProxyHttpService httpService,
                               ParamResolverWrapper paramResolver,
                               ISqlDriver sqlQueryService)
        {
            _paramResolver = paramResolver;
            _sqlQueryService = sqlQueryService;
            _scenarioContext = scenarioContext;
            _pageFactory = pageFactory;
            _waitHelper = waitHelper;
            _valueResolver = templateResolver;
            _httpService = httpService;
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

        //ToDo: сделать регулярку, которая позволит писать много параметров
        [When(@"сохраняю параметр ""(.*)"" из результата запроса ""(.*)""")]
        public void WhenСохраняюПараметрИзРезультатаЗапроса(string paramName, string requestUrl)
        {
            _waitHelper.WaitUntilAllAjaxIsCompleted();
            var paramValue = _paramResolver.Resolve(requestUrl);
            _scenarioContext.Set(paramValue, string.Concat("Param:", paramName).ToUpper());
        }


        [When(@"я подставляю параметр в запрос")]
        public void WhenЯПодставляюПараметрВЗапрос(string sqlRequest)
        {
            var resolvedRequest = _valueResolver.Resolve(sqlRequest);
            _scenarioContext.Add("SqlRequest", resolvedRequest);
        }

        [Then(@"выполняю запрос")]
        public void ThenВыполняюЗапрос(string sqlRequest)
        {
            var queryResult = _sqlQueryService.ExecuteQuery(sqlRequest);
            _scenarioContext.Add("SqlQueryResult", queryResult);
        }


        [Then(@"вижу следующие данные")]
        public void ThenВижуСледующиеДанные(Table table)
        {
            var sqlQueryResult = _scenarioContext.Get<List<Dictionary<string, object>>>("SqlQueryResult");
            table.CompareToCustomTable(sqlQueryResult);
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
