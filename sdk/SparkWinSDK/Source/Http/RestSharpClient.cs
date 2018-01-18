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
using RestSharp;

namespace SparkSDK
{
    internal class RestSharpClient : IServiceRequestClient
    {
        public void Execute<T>(ServiceRequest serviceRequest, Action<SparkApiEventArgs<T>> completedhandler) where T : new()
        {
            if (serviceRequest == null)
            {
                completedhandler?.Invoke(new SparkApiEventArgs<T>(false, null, default(T)));
                return;
            }

            RestRequest request = new RestRequest(serviceRequest.Resource, (Method)serviceRequest.Method);

            if (serviceRequest.AccessToken != null)
            {
                request.AddHeader("Authorization", "Bearer " + serviceRequest.AccessToken);
            }

            foreach (var pair in serviceRequest.Headers)
            {
                request.AddHeader(pair.Key, pair.Value);
            }

            foreach (var pair in serviceRequest.QueryParameters)
            {
                request.AddParameter(pair.Key, pair.Value, ParameterType.GetOrPost);
            }

            foreach (var pair in serviceRequest.BodyParameters)
            {
                request.AddParameter(pair.Key, pair.Value, ParameterType.GetOrPost);
            }

            if (serviceRequest.RootElement.Length != 0)
            {
                request.RootElement = serviceRequest.RootElement;
            }

            var client = new RestClient();
            client.BaseUrl = new System.Uri(serviceRequest.baseUri);

            SDKLogger.Instance.Info($"http request[{serviceRequest.Method.ToString()}]: {serviceRequest.baseUri + request.Resource}" );
            client.ExecuteAsync<T>(request, response =>
            {
                if (response.StatusCode != System.Net.HttpStatusCode.OK
                && response.StatusCode != System.Net.HttpStatusCode.NoContent)
                {
                    string httpRspMsg = response.StatusCode + ":" + response.StatusDescription;
                    SDKLogger.Instance.Error($"http response error: {httpRspMsg}");
                    completedhandler?.Invoke(new SparkApiEventArgs<T>(false, new SparkError(SparkErrorCode.ServiceFailed, httpRspMsg), default(T)));
                    return;
                }

                SDKLogger.Instance.Info("http response success");
                SDKLogger.Instance.Debug("http response content: {0}", response.Content);
                completedhandler?.Invoke(new SparkApiEventArgs<T>(true, null, (T)response.Data));
            });
        }
    }
}
