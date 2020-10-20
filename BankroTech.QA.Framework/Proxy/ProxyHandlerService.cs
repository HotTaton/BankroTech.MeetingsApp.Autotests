using BankroTech.QA.Framework.Helpers;
using Microsoft.Extensions.Configuration;
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
    internal class ProxyHandlerService : IProxyHttpService, IProxyHandlerService, IProxyCookieService, IAuthService, IDisposable
    {
        private static readonly object locker = new object();
        private static readonly string[] _loggingHttpMethods = new string[] { "GET", "POST", "PUT", "DELETE" };
        private readonly Dictionary<string, string> _cookieJar = new Dictionary<string, string>();
        private readonly string _loggingHostName;

        private readonly ConcurrentDictionary<Guid, Request> _httpRequestsHistory = new ConcurrentDictionary<Guid, Request>();
        private readonly ConcurrentDictionary<Guid, Response> _httpResponsesHistory = new ConcurrentDictionary<Guid, Response>();
        private readonly string _authCookieName;
        private bool _disposedValue;

        public ProxyHandlerService(IConfigurationRoot configuration)
        {
            _loggingHostName = configuration.GetSection("ProxyLoggingHostName").Value;
            _authCookieName = configuration.GetSection("AuthCookieName").Value;

            //что если в момент регистрации у нас вызовется BeforeRequest?
            lock (locker)
            {
                ProxyServerContainer.ProxyServer.BeforeRequest += OnRequest;
                ProxyServerContainer.ProxyServer.BeforeResponse += OnResponse;
            }
        }

        public bool IsAuthorized => _cookieJar.ContainsKey(_authCookieName);

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

        public void ClearCookies()
        {
            _cookieJar.Clear();
        }

        public void SetCookie(string key, string value)
        {
            _cookieJar[key] = value;
        }

        private void AddCookieHeader(Request request)
        {
            if (_cookieJar.Any())
            {
                const string COOKIE_HEADER_NAME = "Cookie";

                if (!request.Headers.HeaderExists(COOKIE_HEADER_NAME))
                {
                    var cookieData = string.Join("; ", _cookieJar.Select(kvp => $"{kvp.Key}={kvp.Value}"));
                    request.Headers.AddHeader(COOKIE_HEADER_NAME, cookieData);
                }
            }
        }
                        
        private void SetCookie(Response response)
        {
            const string SET_COOKIE_HEADER_NAME = "Set-Cookie";

            if (response.Headers.HeaderExists(SET_COOKIE_HEADER_NAME))
            {
                var setCookieHeaders = response.Headers.GetHeaders(SET_COOKIE_HEADER_NAME);

                foreach (var header in setCookieHeaders)
                {
                    var headerInfo = header.Value.Split(';');
                    var cookieInfo = new List<(string key, string val)>();

                    foreach (var kvp in headerInfo)
                    {
                        var splittedKvp = kvp.Split('=');
                        var key = splittedKvp[0];
                        var val = splittedKvp.Length > 1 ? splittedKvp[1] : null;
                        cookieInfo.Add((key, val));
                    }

                    var maxAge = int.MaxValue;                    
                    if (cookieInfo.Any(x => x.key.Equals("max-age", StringComparison.OrdinalIgnoreCase)))
                    {
                        maxAge = int.Parse(cookieInfo.First(x => x.key.Equals("max-age", StringComparison.OrdinalIgnoreCase)).val);
                    }

                    var expires = DateTime.MaxValue;
                    if (cookieInfo.Any(x => x.key.Equals("expires", StringComparison.OrdinalIgnoreCase)))
                    {
                        expires = DateTime.Parse(cookieInfo.First(x => x.key.Equals("expires", StringComparison.OrdinalIgnoreCase)).val);
                    }

                    var cookieValue = cookieInfo[0].val?.Trim();

                    var isExpired = maxAge < 1 || expires < DateTime.Now || string.IsNullOrEmpty(cookieValue);
                    
                    var cookieName = cookieInfo[0].key;
                    
                    if (isExpired)
                    {
                        _cookieJar.Remove(cookieName);
                    }
                    else
                    {
                        _cookieJar.Add(cookieName, cookieValue);
                    }
                }
            }
        }

        #region Event delegates
        public async Task OnRequest(object sender, SessionEventArgs eventArgs)
        {
            var request = eventArgs.HttpClient.Request;

            if (!request.RequestUri.Host.Equals(_loggingHostName))
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
        //ToDo: сделать дополнительные проверки на 200 статус (возможно, отдельный метод) + 302
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

                SetCookie(response);
                var sessionGuid = (Guid)eventArgs.UserData;
                _httpResponsesHistory.TryAdd(sessionGuid, response);
            }
        }
        #endregion Event delegates

        public void Dispose()
        {
            if (!_disposedValue)
            {
                lock (locker)
                {
                    ProxyServerContainer.ProxyServer.BeforeRequest -= OnRequest;
                    ProxyServerContainer.ProxyServer.BeforeResponse -= OnResponse;
                }

                _disposedValue = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}
