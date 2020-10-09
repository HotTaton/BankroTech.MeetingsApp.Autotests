using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Titanium.Web.Proxy.Http;

namespace BankroTech.QA.Framework.Proxy
{
    public interface IProxyHttpService
    {
        IReadOnlyDictionary<Guid, Request> HttpRequestsHistory { get; }

        IReadOnlyDictionary<Guid, Response> HttpResponsesHistory { get; }

        IJEnumerable<JToken> GetResponseBody(string url, int requestIndex);

        IJEnumerable<JToken> GetLastResponseBody(string url);
    }
}
