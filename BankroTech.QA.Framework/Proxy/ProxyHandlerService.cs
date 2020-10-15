using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Http;

namespace BankroTech.QA.Framework.Proxy
{
    public class ProxyHandlerService : IProxyHttpService, IProxyHandlerService
    {
        private const string LOGGING_HOST_NAME = "localhost";
        private static readonly string[] _loggingHttpMethods = new string[] { "GET", "POST", "PUT", "DELETE" };
        private readonly Dictionary<string, string> _cookieJar = new Dictionary<string, string>();

        private readonly ConcurrentDictionary<Guid, Request> _httpRequestsHistory = new ConcurrentDictionary<Guid, Request>();
        private readonly ConcurrentDictionary<Guid, Response> _httpResponsesHistory = new ConcurrentDictionary<Guid, Response>();

        public IReadOnlyDictionary<Guid, Request> HttpRequestsHistory => _httpRequestsHistory;
        public IReadOnlyDictionary<Guid, Response> HttpResponsesHistory => _httpResponsesHistory;
        
        public IJEnumerable<JToken> GetResponseBody(string method, string url, int requestIndex)
        {
            var httpResponse = GetResponsesByRequestUrl(url, method).ElementAt(requestIndex);
            return ParseResponseBody(url, httpResponse);
        }

        public IJEnumerable<JToken> GetLastResponseBody(string method, string url)
        {
            var httpResponse = GetResponsesByRequestUrl(method, url).Last();
            return ParseResponseBody(url, httpResponse);
        }

        private static IJEnumerable<JToken> ParseResponseBody(string url, Response httpResponse)
        {
            if (httpResponse.HasBody && !string.IsNullOrEmpty(httpResponse.BodyString))
            {
                return JObject.Parse(httpResponse.BodyString);
            }
            else
            {
                throw new Exception($"Response for {url} hasn't body");
            }
        }

        private IEnumerable<Response> GetResponsesByRequestUrl(string method, string url)
        {
            var requests = _httpRequestsHistory
                                .Where(kvp => kvp.Value.RequestUriString.Contains(url, StringComparison.OrdinalIgnoreCase)
                                                && string.Equals(kvp.Value.Method, method, StringComparison.OrdinalIgnoreCase));

            foreach (var request in requests)
            {
                yield return _httpResponsesHistory[request.Key];
            }
        }

        public void CleanHistory()
        {
            _httpRequestsHistory.Clear();
        }

        public void SetCookie(string key, string value)
        {
            _cookieJar[key] = value;
        }

        private void AddCookieHeader(Request request)
        {
            if (_cookieJar.Any())
            {
                var cookieData = string.Join("; ", _cookieJar.Select(kvp => $"{kvp.Key}={kvp.Value}"));
                request.Headers.AddHeader("Cookie", cookieData);
            }
        }

        #region Event delegates
        public async Task OnRequest(object sender, SessionEventArgs eventArgs)
        {
            var request = eventArgs.HttpClient.Request;

            if (!request.RequestUri.Host.Equals(LOGGING_HOST_NAME))
            {
                return;
            }

            AddCookieHeader(request);
            if (_loggingHttpMethods.Any(method => string.Equals(method, request.Method, StringComparison.OrdinalIgnoreCase)))
            {
                if (request.HasBody)
                {
                    request.KeepBody = true;
                    var requestBody = await eventArgs.GetRequestBody();
                    eventArgs.SetRequestBody(requestBody);
                }

                var sessionGuid = Guid.NewGuid();
                _httpRequestsHistory.TryAdd(sessionGuid, request);

                eventArgs.UserData = sessionGuid;
            }
        }

        //ToDo: check response type (application/json, etc.)
        //ToDo: сделать дополнительные проверки на 200 статус (возможно, отдельный метод)
        public async Task OnResponse(object sender, SessionEventArgs eventArgs)
        {
            if (eventArgs.UserData != null)
            {
                var response = eventArgs.HttpClient.Response;
                if (response.HasBody)
                {
                    response.KeepBody = true;
                    var responseBody = await eventArgs.GetResponseBody();
                    eventArgs.SetResponseBody(responseBody);
                }

                var sessionGuid = (Guid)eventArgs.UserData;
                _httpResponsesHistory.TryAdd(sessionGuid, response);
            }
        }        
        #endregion Event delegates
    }
}
