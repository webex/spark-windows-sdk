#region License
// Copyright (c) 2016-2017 Cisco Systems, Inc.

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SparkSDK
{
    internal enum HttpMethod
    {
        GET = 0,
        POST = 1,
        PUT = 2,
        DELETE = 3,
        HEAD = 4,
        OPTIONS = 5,
        PATCH = 6,
        MERGE = 7
    }


    internal class ServiceRequest
    {
        public readonly string baseUri;
        public HttpMethod Method { get; set; }
        private string resource;
        public string Resource
        {
            get { return resource; }
            set { resource += ("/" + value); }
        }
        public List<KeyValuePair<string, string>> Headers { get; set; }
        public List<KeyValuePair<string, object>> QueryParameters { get; set; }
        public List<KeyValuePair<string, object>> BodyParameters { get; set; }
        IAuthenticator Authenticator { get; set; }
        public string RootElement { get; set; }
        public string AccessToken { get; set; }
        public IServiceRequestClient ClientHandler { get; set; }

        public ServiceRequest()
        {
            Method = HttpMethod.GET;
            baseUri = "https://api.ciscospark.com/v1/";
            AccessToken = null;

            Headers = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Content-Type", "application/json"),
                new KeyValuePair<string, string>("User-Agent", UserAgent.Instance.Name),
                new KeyValuePair<string, string>("Spark-User-Agent", UserAgent.Instance.Name)
            };
            QueryParameters = new List<KeyValuePair<string, object>>();
            BodyParameters = new List<KeyValuePair<string, object>>();
            Resource = "";
            RootElement = "";

            ClientHandler = new RestSharpClient();
        }

        public ServiceRequest(IAuthenticator authenticator)
            : this()
        {
            this.Authenticator = authenticator;
        }

        public ServiceRequest(IAuthenticator authenticator, HttpMethod method) 
            :this()
        {
            this.Authenticator = authenticator;
            this.Method = method;
        }

        public void AddQueryParameters(string name, object value)
        {
            QueryParameters.Add(new KeyValuePair<string, object>(name, value));
        }

        public void AddBodyParameters(string name, object value)
        {
            BodyParameters.Add(new KeyValuePair<string, object>(name, value));
        }

        public void AddHeaders(string name, string value)
        {
            Headers.Add(new KeyValuePair<string, string>(name, value));
        }

        

        public void Execute<T>(Action<SparkApiEventArgs<T>> completedhandler) where T:new()
        {
            if (Authenticator != null)
            {
                Authenticator.AccessToken(response =>
                {
                    if (response == null || response.IsSuccess == false || response.Data == null)
                    {
                        SDKLogger.Instance.Error("ServiceRequest.Execute.accessToken Failed");
                        completedhandler?.Invoke(new SparkApiEventArgs<T>(false, null, default(T)));
                        return;
                    }

                    this.AccessToken = (string)response.Data;

                    ClientHandler.Execute<T>(this, resp=>
                    {
                        completedhandler?.Invoke(resp);
                        return;
                    });

                });
            }
            else
            {
                ClientHandler.Execute<T>(this, resp =>
                {
                    completedhandler?.Invoke(resp);
                    return;
                });

            }
        }
    }
}
