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
using System.Threading.Tasks;

namespace SparkSDK
{
    /// <summary>
    /// audio and video IO device type.
    /// </summary>
    /// Since: 0.1.0
    public enum AVIODeviceType
    {
        /// <summary>
        /// Invalid device.
        /// </summary>
        /// Since: 0.1.0
        Invalid = 0,
        /// <summary>
        /// This is microphone.
        /// </summary>
        /// Since: 0.1.0
        Microphone = 1,
        /// <summary>
        /// This is speaker.
        /// </summary>
        /// Since: 0.1.0
        Speaker = 2,
        /// <summary>
        /// This is camera.
        /// </summary>
        /// Since: 0.1.0
        Camera = 3,
        /// <summary>
        /// This is ringer.
        /// </summary>
        /// Since: 0.1.0
        Ringer = 4
    }

    /// <summary>
    /// Audio and video IO device.
    /// </summary>
    /// Since: 0.1.0
    public class AVIODevice
    {
        /// <summary>
        /// If this is default device.
        /// </summary>
        /// Since: 0.1.0
        public bool DefaultDevice { get; set; }

        /// <summary>
        /// IO device ID.
        /// </summary>
        /// Since: 0.1.0
        public string Id { get; set; }

        /// <summary>
        /// IO device name.
        /// </summary>
        /// Since: 0.1.0
        public string Name { get; set; }

        /// <summary>
        /// IO device type.
        /// </summary>
        /// Since: 0.1.0
        public AVIODeviceType Type { get; set; }
    }
}
