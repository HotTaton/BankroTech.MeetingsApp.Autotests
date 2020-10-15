using BankroTech.QA.Framework.API;
using BankroTech.QA.Framework.Proxy;
using BankroTech.QA.Framework.SqlDriver;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Tests.StepDefinitions
{
    //ToDo можно обернуть какой-нибудь сервис-враппер над IWebDriver и хранить там флаг авторизации
    [Binding]
    public class AuthDefinition
    {
        private readonly FeatureContext _featureContext;
        private readonly IProxyHandlerService _proxyHandler;
        private readonly IRestClientService _restClient;
        private readonly ISqlDriver _sqlQueryService;

        public AuthDefinition(FeatureContext featureContext,                              
                              IProxyHandlerService proxyHandler,
                              IRestClientService restClient,
                              ISqlDriver sqlQueryService)
        {
            _featureContext = featureContext;            
            _proxyHandler = proxyHandler;
            _restClient = restClient;
            _sqlQueryService = sqlQueryService;
        }

        [Given(@"я авторизованный пользователь")]
        public void GivenАвторизованныйПользователь()
        {
            if (_featureContext.ContainsKey("Auth"))
                return;
                        
            _restClient.PostRequest("/account/login", "{ \"PhoneNumber\": \"79171864323\", \"Password\": \"12345678\" }");
            var queryResult = _sqlQueryService.ExecuteQuery("SELECT \"Value\" FROM \"AccountTokens\" ORDER BY \"ExpiresAt\" DESC LIMIT 1");
            var response = _restClient.PostRequest("/account/verifyCode", "{ \"Code\": \"" + queryResult[0]["Value"] + "\" }");
            var cookies = response.Cookies;

            foreach (var cookie in cookies)
            {
                _proxyHandler.SetCookie(cookie.Name, cookie.Value);
            }
        }
    }
}
