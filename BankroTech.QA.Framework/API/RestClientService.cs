﻿using BankroTech.QA.Framework.Helpers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Net;

namespace BankroTech.QA.Framework.API
{
    internal class RestClientService : IRestClientService
    {
        private readonly IRestClient _client;

        public RestClientService(IConfigurationRoot configuration, IWebContextInfo contextInfo)
        {
            var applicationName = configuration.GetSection("ApplicationName").Value;            

            _client = new RestClient(applicationName)
            {
                Proxy = new WebProxy
                {
                    Address = new Uri($"http://localhost:{contextInfo.Port}"),
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = true
                },
                CookieContainer = new CookieContainer()
            };
        }

        public IRestResponse PostRequest(string action, string body = null)
        {
            var request = new RestRequest(action, Method.POST, DataFormat.Json);
            if (!string.IsNullOrEmpty(body))
            {
                request.AddJsonBody(body);
            }

            var response = _client.Execute(request);
            return response;
        }

        public JObject PostRequestAndDeseriallize(string action, string body = null)
        {
            var response = PostRequest(action, body);

            return JObject.Parse(response.Content);
        }
    }
}
