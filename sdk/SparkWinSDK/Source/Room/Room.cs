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
    /// The enumeration of the types of a room. 
    /// </summary>
    /// <remarks>Since: 0.1.0</remarks>
    public enum RoomType
    {
        /// <summary>
        /// 1-to-1 room between two people
        /// </summary>
        /// <remarks>Since: 0.1.0</remarks>
        Direct,

        /// <summary>
        /// Group room among multiple people
        /// </summary>
        /// <remarks>Since: 0.1.0</remarks>
        Group
    }


    /// <summary>
    /// A data type represents a Room at Cisco Spark cloud.
    /// </summary>
    /// note: Room has been renamed to Space in Cisco Spark. 
    /// <remarks>Since: 0.1.0</remarks>
    public class Room
    {
        /// <summary>
        /// The identifier of this room.
        /// </summary>
        /// <remarks>Since: 0.1.0</remarks>
        public string Id { get; set; }


        /// <summary>
        /// The title of this room.
        /// </summary>
        /// <remarks>Since: 0.1.0</remarks>
        public string Title { get; set; }

        /// <summary>
        /// The type of this room.
        /// </summary>
        /// <remarks>Since: 0.1.0</remarks>
        public RoomType Type { get; set; }


        /// <summary>
        /// Indicate if this room is locked.
        /// </summary>
        /// <remarks>Since: 0.1.0</remarks>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Last activity of this room.
        /// </summary>
        /// <remarks>Since: 0.1.0</remarks>
        public string LastActivity { get; set; }

        /// <summary>
        /// The timestamp that this room being created.
        /// </summary>
        /// <remarks>Since: 0.1.0</remarks>
        public DateTime Created { get; set; }


        /// <summary>
        /// The team Id that this room associated with.
        /// </summary>
        /// <remarks>Since: 0.1.0</remarks>
        public string TeamId { get; set; }
    }
}
