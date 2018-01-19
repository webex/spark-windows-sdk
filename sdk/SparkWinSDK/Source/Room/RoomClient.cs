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
    /// Class RoomClient contains APIs which are used to manage the rooms themselves. 
    /// Rooms are created and deleted with this API. You can also update a room to change its title 
    /// </summary>
    /// <remarks>Since: 0.1.0</remarks>
    public sealed class RoomClient
    {
        readonly IAuthenticator authenticator;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomClient"/> class.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <remarks>Since: 0.1.0</remarks>
        public RoomClient(IAuthenticator authenticator )
        {
            this.authenticator = authenticator;
        }

        private ServiceRequest BuildRequest()
        {
            var request = new ServiceRequest(authenticator)
            {
                Resource = "rooms",
            };
            return request;
        }

        /// <summary>
        /// Lists all rooms where the authenticated user belongs.
        /// </summary>
        /// <param name="teamId">If not null, only list the rooms that are associated with the team by team id.</param>
        /// <param name="max">The maximum number of rooms in the response. If null, all rooms are listed.</param>
        /// <param name="type">If not null, only list the rooms of this type. Otherwise all rooms are listed.</param>
        /// <param name="completionHandler">The completion handler.</param>
        /// <remarks>Since: 0.1.0</remarks>
        public void List(string teamId, int? max, RoomType? type, Action<SparkApiEventArgs<List<Room>>> completionHandler)
        {
            ServiceRequest request = BuildRequest();
            request.Method = HttpMethod.GET;
            request.RootElement = "items";
            if (teamId != null) request.AddQueryParameters("teamId", teamId);
            if (max != null) request.AddQueryParameters("max", max);
            if (type != null) request.AddQueryParameters("type", type.ToString().ToLower());

            request.Execute<List<Room>>(completionHandler);

        }

        /// <summary>
        /// Creates a room. The authenticated user is automatically added as a member of the room. See the Memberships API to learn how to add more people to the room.
        /// </summary>
        /// <param name="title">A user-friendly name for the room.</param>
        /// <param name="teamId">If not null, this room will be associated with the team by team id. Otherwise, this room is not associated with any team.</param>
        /// <param name="completionHandler">The completion event handler.</param>
        /// <remarks>Since: 0.1.0</remarks>
        public void Create( string title, string teamId, Action<SparkApiEventArgs<Room>> completionHandler)
        {
            ServiceRequest request = BuildRequest();
            request.Method = HttpMethod.POST;
            if (title != null) request.AddBodyParameters("title", title);
            if (teamId != null) request.AddBodyParameters("teamId", teamId);

            request.Execute<Room>(completionHandler);
        }

        /// <summary>
        /// Retrieves the details for a room by id.
        /// </summary>
        /// <param name="roomId">The identifier of the room.</param>
        /// <param name="completionHandler">The completion event handler.</param>
        /// <remarks>Since: 0.1.0</remarks>
        public void Get(string roomId, Action<SparkApiEventArgs<Room>> completionHandler)
        {
            ServiceRequest request = BuildRequest();
            request.Method = HttpMethod.GET;
            request.Resource = roomId;

            request.Execute<Room>(completionHandler);
        }

        /// <summary>
        /// Updates the details for a room by id.
        /// </summary>
        /// <param name="roomId">The identifier of the room.</param>
        /// <param name="title">A user-friendly name for the room.</param>
        /// <param name="completionHandler">The completion event handler.</param>
        /// <remarks>Since: 0.1.0</remarks>
        public void Update(string roomId, string title, Action<SparkApiEventArgs<Room>> completionHandler)
        {
            ServiceRequest request = BuildRequest();
            request.Method = HttpMethod.PUT;
            request.Resource = roomId;
            if (title != null) request.AddQueryParameters("title", title);

            request.Execute<Room>(completionHandler);
        }

        /// <summary>
        /// Deletes a room by id.
        /// </summary>
        /// <param name="roomId">The identifier of the room.</param>
        /// <param name="completionHandler">The completion event handler.</param>
        /// <remarks>Since: 0.1.0</remarks>
        public void Delete(string roomId, Action<SparkApiEventArgs> completionHandler)
        {
            ServiceRequest request = BuildRequest();
            request.Method = HttpMethod.DELETE;
            request.Resource = roomId;

            request.Execute<bool>(completionHandler);
        }

    }
}
