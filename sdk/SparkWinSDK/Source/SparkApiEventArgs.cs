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
    /// Represents the Spark SDK api event args.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    /// <remarks>Since: 0.1.0</remarks>
    public class SparkApiEventArgs : EventArgs
    {
        /// <summary>
        /// Indicating whether this event is success.
        /// </summary>
        protected readonly bool isSuccess;
        /// <summary>
        /// Indicating the error value, if the success is false, otherwise is null
        /// </summary>
        protected readonly SparkError error;

        /// <summary>
        /// Initializes a new instance of the <see cref="SparkApiEventArgs"/> class.
        /// </summary>
        /// <remarks>Since: 0.1.0</remarks>
        public SparkApiEventArgs() : this(false, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SparkApiEventArgs"/> class.
        /// </summary>
        /// <param name="isSuccess">if set to <c>true</c> [success].</param>
        /// <param name="error">The error value if the successis is false, `null` otherwise. <see cref="SparkError"/></param>
        /// <remarks>Since: 0.1.0</remarks>
        public SparkApiEventArgs(bool isSuccess, SparkError error)
        {
            this.isSuccess = isSuccess;
            this.error = error;
        }

        /// <summary>
        /// Gets a value indicating whether this event is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>Since: 0.1.0</remarks>
        public bool IsSuccess
        {
            get { return this.isSuccess; }
        }

        /// <summary>
        /// Gets the error value, if the success is false, otherwise is null. <see cref="SparkError"/>
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        /// <remarks>Since: 0.1.0</remarks>
        public SparkError Error
        {
            get { return this.error; }
        }
    }
    /// <summary>
    /// Represents the Spark SDK api event args.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="SparkApiEventArgs" />
    /// <remarks>Since: 0.1.0</remarks>
    public sealed class SparkApiEventArgs<T> : SparkApiEventArgs
    {
        private readonly T data;

        /// <summary>
        /// Initializes a new instance of the <see cref="SparkApiEventArgs{T}"/> class.
        /// </summary>
        /// <remarks>Since: 0.1.0</remarks>
        public SparkApiEventArgs() : this(false, null, default(T))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SparkApiEventArgs{T}"/> class.
        /// </summary>
        /// <param name="isSuccess">if set to <c>true</c> [success].</param>
        /// <param name="error">The error value if the successis is false, `null` otherwise. <see cref="SparkError"/></param>
        /// <param name="data">The data if the successis is true, 'null' otherwise.</param>
        /// <remarks>Since: 0.1.0</remarks>
        public SparkApiEventArgs(bool isSuccess, SparkError error, T data):base(isSuccess,error)
        {
            this.data = data;
        }

        /// <summary>
        /// Gets the data, if the success is true, otherwise is default value of T type.
        /// The type of data is T.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        /// <remarks>Since: 0.1.0</remarks>
        public T Data
        {
            get { return this.data; }
        }

    }

}
