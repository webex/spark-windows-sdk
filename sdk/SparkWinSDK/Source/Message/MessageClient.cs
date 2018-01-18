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

namespace SparkSDK
{
    /// <summary>
    /// Messages are how we communicate in a room. In Spark, each message is displayed on its own line along with a timestamp and sender information. 
    /// Use this API to list, create, and delete messages. Message can contain plain text, rich text, and a file attachment.
    /// </summary>
    /// <remarks>Since: 0.1.0</remarks>
    public sealed class MessageClient
    {
        readonly IAuthenticator authenticator;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageClient"/> class.
        /// </summary>
        /// <remarks>Since: 0.1.0</remarks>
        public MessageClient() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageClient"/> class.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <remarks>Since: 0.1.0</remarks>
        public MessageClient(IAuthenticator authenticator )
        {
            this.authenticator = authenticator;
        }

        private ServiceRequest BuildRequest()
        {
            var request = new ServiceRequest(authenticator)
            {
                Resource = "messages",
            };            

            return request;
        }

        /// <summary>
        /// Lists all messages in a room by room Id.
        /// If present, it includes the associated media content attachment for each message.
        /// The list sorts the messages in descending order by creation date.
        /// </summary>
        /// <param name="roomId">The identifier of the room.</param>
        /// <param name="before">If not null, only list messages sent only before this date and time, in ISO8601 format.</param>
        /// <param name="beforeMessage">if not null, only list messages sent only before this message by id.</param>
        /// <param name="max">The maximum number of messages in the response.</param>
        /// <param name="completionHandler">The completion event handler.</param>
        /// <remarks>Since: 0.1.0</remarks>
        public void List(string roomId, string before, string beforeMessage, int? max, Action<SparkApiEventArgs<List<Message>>> completionHandler)
        {
            ServiceRequest request = BuildRequest();
            request.Method = HttpMethod.GET;
            if (roomId != null)         request.AddQueryParameters("roomId", roomId);
            if (before != null)         request.AddQueryParameters("before", before);
            if (beforeMessage != null)  request.AddQueryParameters("beforeMessage", beforeMessage);
            if (max != null)            request.AddQueryParameters("max", max);
            request.RootElement = "items";

            request.Execute<List<Message>>(completionHandler);
        }

        /// <summary>
        /// Posts a plain text message, and optionally, a media content attachment, to a room by room Id.
        /// </summary>
        /// <param name="roomId">The identifier of the room where the message is to be posted.</param>
        /// <param name="text">The plain text message to be posted to the room.</param>
        /// <param name="files">A public URL that Cisco Spark can use to fetch attachments. Currently supports only a single URL. Cisco Spark downloads the content from the URL one time shortly after the message is created and automatically converts it to a format that all Cisco Spark clients can render.</param>
        /// <param name="completionHandler">The completion event handler.</param>
        /// <remarks>Since: 0.1.0</remarks>
        public void PostToRoom(string roomId, string text, string files = null, Action<SparkApiEventArgs<Message>> completionHandler = null)
        {
            Post(roomId, null, null, text, files, completionHandler);
        }

        /// <summary>
        /// Posts a private 1:1 message in plain text, and optionally, a media content attachment, to a person by person Id.
        /// </summary>
        /// <param name="toPersonId">The identifier of the recipient of this private 1:1 message.</param>
        /// <param name="text">The plain text message to post to the recipient.</param>
        /// <param name="files">A public URL that Cisco Spark can use to fetch attachments. Currently supports only a single URL. Cisco Spark  downloads the content from the URL one time shortly after the message is created and automatically converts it to a format that all Cisco Spark clients can render.</param>
        /// <param name="completionHandler">The completion event handler.</param>
        /// <remarks>Since: 0.1.0</remarks>
        public void PostToPersonByID(string toPersonId, string text, string files = null, Action<SparkApiEventArgs<Message>> completionHandler = null)
        {
            Post(null, toPersonId, null, text, files, completionHandler);
        }

        /// <summary>
        /// Posts a private 1:1 message in plain text, and optionally, a media content attachment, to a person by person email.
        /// </summary>
        /// <param name="toPersonEmail">The email address of the recipient when sending a private 1:1 message.</param>
        /// <param name="text">The plain text message to post to the room.</param>
        /// <param name="files">A public URL that Spark can use to fetch attachments. Currently supports only a single URL. The Spark Cloud downloads the content one time shortly after the message is created and automatically converts it to a format that all Spark clients can render.</param>
        /// <param name="completionHandler">The completion event handler.</param>
        /// <remarks>Since: 0.1.0</remarks>
        public void PostToPerson(string toPersonEmail, string text, string files = null, Action<SparkApiEventArgs<Message>> completionHandler = null)
        {
            Post(null, null, toPersonEmail, text, files, completionHandler);
        }

        private void Post(string roomId, string toPersonId, string toPersonEmail, string text, string files, Action<SparkApiEventArgs<Message>> completionHandler)
        {
            ServiceRequest request = BuildRequest();
            request.Method = HttpMethod.POST;
            if (roomId != null)         request.AddBodyParameters("roomId", roomId);
            if (toPersonId != null)     request.AddBodyParameters("toPersonId", toPersonId);
            if (toPersonEmail != null)  request.AddBodyParameters("toPersonEmail", toPersonEmail);
            if (text != null)           request.AddBodyParameters("text", text);
            if (files != null)          request.AddBodyParameters("files", files);

            request.Execute<Message>(completionHandler);
        }

        /// <summary>
        /// Retrieves the details for a message by message Id.
        /// </summary>
        /// <param name="messageId">The identifier of the message.</param>
        /// <param name="completionHandler">The completion event handler.</param>
        /// <remarks>Since: 0.1.0</remarks>
        public void Get(string messageId, Action<SparkApiEventArgs<Message>> completionHandler)
        {
            ServiceRequest request = BuildRequest();
            request.Method = HttpMethod.GET;
            request.Resource = messageId;

            request.Execute<Message>(completionHandler);
        }

        /// <summary>
        /// Deletes a message by message id.
        /// </summary>
        /// <param name="messageId">The identifier of the message.</param>
        /// <param name="completionHandler">The completion event handler.</param>
        /// <remarks>Since: 0.1.0</remarks>
        public void Delete(string messageId, Action<SparkApiEventArgs> completionHandler)
        {
            ServiceRequest request = BuildRequest();
            request.Method = HttpMethod.DELETE;
            request.Resource = messageId;

            request.Execute<bool>(completionHandler);
        }
    }
}
