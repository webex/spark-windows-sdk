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
    /// The struct of a Message on Cisco Spark.
    /// </summary>
    /// <remarks>Since: 0.1.0</remarks>
    public class Message
    {
        /// <summary>
        /// The identifier of this message.
        /// </summary>
        /// <remarks>Since: 0.1.0</remarks>
        public string Id { get; set; }

        /// <summary>
        /// The identifier of the person who sent this message.
        /// </summary>
        /// <remarks>Since: 0.1.0</remarks>
        public string PersonId { get; set; }

        /// <summary>
        /// The email address of the person who sent this message.
        /// </summary>
        /// <remarks>Since: 0.1.0</remarks>
        public string PersonEmail { get; set; }

        /// <summary>
        /// The identifier of the room where this message was posted.
        /// </summary>
        /// <remarks>Since: 0.1.0</remarks>
        public string RoomId { get; set; }

        /// <summary>
        /// The content of the message in plain text. This can be the alternate text if markdown is specified.
        /// </summary>
        /// <remarks>Since: 0.1.0</remarks>
        public string Text { get; set; }

        /// <summary>
        /// A array of public URLs of the attachments in the message.
        /// </summary>
        /// <remarks>Since: 0.1.0</remarks>
        public List<string> Files { get; set; }

        /// <summary>
        /// The identifier of the recipient when sending a private 1:1 message.
        /// </summary>
        /// <remarks>Since: 0.1.0</remarks>
        public string ToPersonId { get; set; }

        /// <summary>
        /// The email address of the recipient when sending a private 1:1 message.
        /// </summary>
        /// <remarks>Since: 0.1.0</remarks>
        public string ToPersonEmail { get; set; }

        /// <summary>
        /// The timestamp that the message being created.
        /// </summary>
        /// <remarks>Since: 0.1.0</remarks>
        public DateTime Created { get; set; }
    }
}
