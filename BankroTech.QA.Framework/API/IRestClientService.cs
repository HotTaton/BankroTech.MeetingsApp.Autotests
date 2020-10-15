using Newtonsoft.Json.Linq;
using RestSharp;

namespace BankroTech.QA.Framework.API
{
    public interface IRestClientService
    {
        IRestResponse PostRequest(string action, string body = null);
        JObject PostRequestAndDeseriallize(string action, string body = null);
    }
}