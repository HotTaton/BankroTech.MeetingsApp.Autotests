using BankroTech.QA.Framework.API;
using BankroTech.QA.Framework.Proxy;
using BankroTech.QA.Framework.SqlDriver;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Tests.StepDefinitions
{
    [Binding]
    public class AuthDefinition
    {        
        private readonly IAuthService _authService;
        private readonly IRestClientService _restClient;
        private readonly ISqlDriver _sqlQueryService;

        public AuthDefinition(IAuthService authService,
                              IRestClientService restClient,
                              ISqlDriver sqlQueryService)
        {            
            _authService = authService;
            _restClient = restClient;
            _sqlQueryService = sqlQueryService;
        }

        //ToDo убрать зависимость от контекста
        [Given(@"я авторизованный пользователь")]
        public void GivenАвторизованныйПользователь()
        {
            if (_authService.IsAuthorized)
                return;
                        
            _restClient.PostRequest("/account/login", "{ \"PhoneNumber\": \"79171864323\", \"Password\": \"12345678\" }");
            var queryResult = _sqlQueryService.ExecuteQuery("SELECT \"Value\" FROM \"AccountTokens\" ORDER BY \"ExpiresAt\" DESC LIMIT 1");
            _restClient.PostRequest("/account/verifyCode", "{ \"Code\": \"" + queryResult[0]["Value"] + "\" }");            
        }
    }
}
