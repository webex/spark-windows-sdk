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
    /// A data type represents a error.
    /// </summary>
    /// Since: 0.1.0
    public class SparkError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SparkError"/> class.
        /// <param name="errorCode"><see cref="ErrorCode"/></param>
        /// <param name="reason">the detail reason of the error.</param>
        /// </summary>
        /// Since: 0.1.0
        public SparkError(SparkErrorCode errorCode, string reason)
        {
            this.ErrorCode = errorCode;
            this.Reason = reason;
        }

        /// <summary>
        /// The error code <see cref="ErrorCode"/>
        /// </summary>
        /// Since: 0.1.0
        public SparkErrorCode ErrorCode
        {
            get;
            private set;
        }

        /// <summary>
        /// The reason of error.
        /// </summary>
        /// Since: 0.1.0
        public string Reason
        {
            get;
            private set;
        }
    }

    /// <summary>
    /// The enumeration of error types.
    /// </summary>
    /// Since: 0.1.0
    public enum SparkErrorCode
    {

        /// <summary>
        /// A service request to Cisco Spark cloud has failed.
        /// </summary>
        /// Since: 0.1.0
        serviceFailed,
        /// <summary>
        /// The Phone has not been registered.
        /// </summary>
        /// Since: 0.1.0
        unregistered,
        /// <summary>
        /// The media requires H.264 codec.
        /// </summary>
        /// Since: 0.1.0
        requireH264,
        /// <summary>
        /// The DTMF is invalid.
        /// </summary>
        /// Since: 0.1.0
        invalidDTMF,
        /// <summary>
        /// The DTMF is unsupported.
        /// </summary>
        /// Since: 0.1.0
        unsupportedDTMF,
        /// <summary>
        /// The service request is illegal.
        /// </summary>
        /// Since: 0.1.0
        illegalOperation,
        /// <summary>
        /// The service is in an illegal status.
        /// </summary>
        /// Since: 0.1.0
        illegalStatus,
    }
}
