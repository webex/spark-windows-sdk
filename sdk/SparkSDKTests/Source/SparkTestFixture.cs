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
using System.Threading;
using RestSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace SparkSDK.Tests
{

    [TestClass]
    public class AssemblyFixture
    {
        [AssemblyInitialize]
        public static void AssemblySetup(TestContext context)
        {
            Console.WriteLine("Assembly Initialize.");
            var fixture = SparkTestFixture.Instance;
        }

        [AssemblyCleanup]
        public static void AssemblyTeardown()
        {
            Console.WriteLine("Assembly Cleanup.");
            SparkTestFixture.Instance.UnLoad();
            Thread.Sleep(10000);
        }
    }

    public class TestUser
    {
        public string AccessToken { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string OrgId { get; set; }
        public string PersonId { get; set; }
    }

    public class SparkTestFixture
    {
        private static volatile SparkTestFixture instance;
        private static readonly object lockHelper = new object();

        public Spark spark;
        public Spark jwtSpark;

        private JWTAuthenticator jwtAuth;
        private string adminClientId;
        private string adminClientSecret;
        private string adminAccessToken;
        //public SparkSDK.SPARK spark;
        public TestUser selfUser;
        public Phone phone;

        class SparkTestFixtureAuth : SparkSDK.IAuthenticator
        {
            private string token;

            public SparkTestFixtureAuth(string token)
            {
                this.token = token;
            }
            public void Authorized(Action<SparkApiEventArgs> completionHandler)
            {
                completionHandler(new SparkApiEventArgs(true, null));
            }
            public void Deauthorize()
            {
            }
            public void AccessToken(Action<SparkSDK.SparkApiEventArgs<string>> completionHandler)
            {
                completionHandler(new SparkSDK.SparkApiEventArgs<string>(true, null, token));
            }
        }

        class AccessToken
        {
            public int Expires_in { get; set; }
            public string Token_type { get; set; }
            public string Access_token { get; set; }
        }


        public SparkTestFixture()
        {
            adminClientId = ConfigurationManager.AppSettings["AdminClientID"] ?? "";
            adminClientSecret = ConfigurationManager.AppSettings["AdminSecret"] ?? "";

            // get access token
            adminAccessToken = CreateAdminAccessToken(adminClientId, adminClientSecret);
            if (adminAccessToken == null)
            {
                Console.WriteLine("Error: create access token failed!");
                return;
            }

            //create the first user
            selfUser = CreateUser(adminAccessToken, adminClientId, adminClientSecret);
            if (selfUser == null)
            {
                Console.WriteLine("Error: create test user failed!");
                return;
            }


            spark = CreateSpark();

            Console.WriteLine("SparkTestFixture build success!");
        }

        public Spark CreateSpark()
        {
            if (spark == null)
            {
                spark = new SparkSDK.Spark(new SparkTestFixtureAuth(selfUser.AccessToken));
            }

            return spark;
        }

        public Spark CreateSparkbyJwt()
        {
            if (jwtSpark == null)
            {
                jwtAuth = new JWTAuthenticator();
                jwtSpark = new SparkSDK.Spark(jwtAuth);

                //login
                for (int count = 1; count <= 5; count++)
                {
                    if (JWtLogin() == true)
                    {
                        Console.WriteLine("JWtLogin success.");
                        break;
                    }
                    Console.WriteLine($"Error: jwt login fail[{count}].");

                    if (count == 5)
                    {
                        jwtSpark = null;
                        return null;
                    }
                }
            }

            return jwtSpark;
        }


        private static string CreateAdminAccessToken(string clientId, string clientSecret)
        {
            RestRequest request = new RestSharp.RestRequest(Method.POST);

            byte[] encodedByte = Encoding.UTF8.GetBytes(string.Format("{0}:{1}", clientId, clientSecret));
            string adminCredentials = Convert.ToBase64String(encodedByte);

            request.AddHeader("Authorization", "Basic " + adminCredentials);
            request.AddParameter("grant_type", "client_credentials", ParameterType.GetOrPost);
            request.AddParameter("scope", "webexsquare:admin Identity:SCIM", ParameterType.GetOrPost);

            var client = new RestClient();
            client.BaseUrl = new System.Uri("https://idbroker.webex.com/idb/oauth2/v1/access_token");

            var response = client.Execute<AccessToken>(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK
            && response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine($"Error: create access token response: {response.StatusDescription}");
                return null;
            }

            return response.Data.Access_token;
        }

        private static TestUser CreateUser(string adminAccessToken, string adminClientId, string adminClientSecret)
        {
            string[] entitlements = { "spark", "webExSquared", "squaredCallInitiation", "squaredTeamMember", "squaredRoomModeration" };
            string scopes = "spark:people_read spark:rooms_read spark:rooms_write spark:memberships_read spark:memberships_write spark:messages_read spark:messages_write spark:teams_write spark:teams_read spark:team_memberships_write spark:team_memberships_read";
            string userName = Guid.NewGuid().ToString();
            string email = userName + "@squared.example.com";

            RestRequest request = new RestSharp.RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + adminAccessToken);
            request.AddJsonBody(new
            {
                clientId = adminClientId,
                clientSecret = adminClientSecret,
                emailTemplate = email,
                displayName = userName,
                password = "P@ssw0rd123",
                entitlements = entitlements,
                authCodeOnly = "false",
                scopes = scopes,
            });

            var client = new RestClient();
            client.BaseUrl = new System.Uri("https://conv-a.wbx2.com/conversation/api/v1/users/test_users_s");

            var response = client.Execute<SparkUser>(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK
            && response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine($"Error: createUser response: {response.StatusCode} {response.StatusDescription}");
                return null;
            }

            return new TestUser
            {
                AccessToken = response.Data.token.access_token,
                Email = response.Data.user.email,
                Name = response.Data.user.name,
                OrgId = response.Data.user.orgId,
                PersonId = GetPersonIdFromUserId(response.Data.user.id),
            };
        }

        public static string GetPersonIdFromUserId(string userId)
        {
            byte[] encodedByte = Encoding.UTF8.GetBytes("ciscospark://us/PEOPLE/" + userId);
            return Convert.ToBase64String(encodedByte).Replace("=", "");
        }

        

        public TestUser CreatUser()
        {
            return CreateUser(adminAccessToken, adminClientId, adminClientSecret);
        }

        public Room CreateRoom(string title)
        {
            var completion = new ManualResetEvent(false);
            var response = new SparkApiEventArgs<Room>();
            spark.Rooms.Create(title, null, rsp =>
            {
                response = rsp;
                completion.Set();
            });

            if (false == completion.WaitOne(30000))
            {
                return null;
            }

            if (response.IsSuccess == true)
            {
                return response.Data;
            }

            return null;
        }

        public bool DeleteRoom(string roomId)
        {
            var completion = new ManualResetEvent(false);
            var response = new SparkApiEventArgs();
            spark.Rooms.Delete(roomId, rsp =>
            {
                response = rsp;
                completion.Set();
            });

            if (false == completion.WaitOne(30000))
            {
                return false;
            }

            if (response.IsSuccess == true)
            {
                return true;
            }

            return false;
        }

        public Membership CreateMembership(string roomId, string email, string personId, bool isModerator)
        {
            var completion = new ManualResetEvent(false);
            var response = new SparkApiEventArgs<Membership>();
            if (email != null)
            {
                spark.Memberships.CreateByPersonEmail(roomId, email, isModerator, rsp =>
                {
                    response = rsp;
                    completion.Set();
                });
            }
            else
            {
                spark.Memberships.CreateByPersonId(roomId, personId, isModerator, rsp =>
                {
                    response = rsp;
                    completion.Set();
                });
            }


            if (false == completion.WaitOne(30000))
            {
                return null;
            }

            if (response.IsSuccess == true)
            {
                return response.Data;
            }

            return null;
        }

        public Team CreateTeam(string teamName)
        {
            var completion = new ManualResetEvent(false);
            var response = new SparkApiEventArgs<Team>();
            spark.Teams.Create(teamName, rsp =>
            {
                response = rsp;
                completion.Set();
            });

            if (false == completion.WaitOne(30000))
            {
                return null;
            }

            if (response.IsSuccess == true)
            {
                return response.Data;
            }

            return null;
        }

        public bool DeleteTeam(string teamId)
        {
            var completion = new ManualResetEvent(false);
            var response = new SparkApiEventArgs();
            spark.Teams.Delete(teamId, rsp =>
            {
                response = rsp;
                completion.Set();
            });

            if (false == completion.WaitOne(30000))
            {
                return false;
            }

            if (response.IsSuccess == true)
            {
                return true;
            }

            return false;
        }

        public static SparkTestFixture Instance
        {
            get
            {
                if (null == instance)
                {
                    lock (lockHelper)
                    {
                        if (null == instance)
                        {
                            instance = new SparkTestFixture();
                        }
                    }

                }
                return instance;
            }
        }

        public void UnLoad()
        {
            if (instance == null)
            {
                return;
            }
            if (spark != null)
            {
                spark.Authenticator.Deauthorize();
            }
            if (jwtSpark != null)
            {
                jwtSpark.Authenticator.Deauthorize();
            }
            
            instance = null;
            Console.WriteLine("fixture unloaded");
        }


        private bool JWtLogin()
        {
            string jwt = ConfigurationManager.AppSettings["JWT"] ?? "";
            var completion = new ManualResetEvent(false);
            var response = new SparkApiEventArgs();

            jwtAuth.AuthorizeWith(jwt, rsp =>
            {
                response = rsp;
                completion.Set();
            });

            if (false == completion.WaitOne(30000))
            {
                Console.WriteLine("authorizeWith timeout");
                return false;
            }

            return response.IsSuccess;
        }

        

        private bool OAuthLogin()
        {
            return false;
        }

        private Person GetMe()
        {
            var completion = new ManualResetEvent(false);
            var response = new SparkApiEventArgs<Person>();
            spark.People.GetMe(rsp =>
            {
                response = rsp;
                completion.Set();
            });

            if (false == completion.WaitOne(30000))
            {
                return null;
            }

            if (response.IsSuccess)
            {
                return response.Data;
            }
            return null;
        }
    }








    public class Locale
    {
        public string languageCode { get; set; }
        public string countryCode { get; set; }
    }

    public class SipAddresses
    {
        public string type { get; set; }
        public string value { get; set; }
        public string primary { get; set; }
    }

    public class UserSettings
    {
        public string sparkSignUpDate { get; set; }
    }


    public class User
    {
        public string id { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string givenName { get; set; }
        public string type { get; set; }
        public List<string> entitlements { get; set; }
        public List<string> roles { get; set; }
        public List<string> photos { get; set; }
        public List<string> ims { get; set; }
        public List<string> phoneNumbers { get; set; }
        public string orgId { get; set; }
        public string isPartner { get; set; }
        public Locale locale { get; set; }
        public List<SipAddresses> sipAddresses { get; set; }
        public UserSettings userSettings { get; set; }
        public List<string> accountStatus { get; set; }
    }

    public class Token
    {
        public string token_type { get; set; }
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string refresh_token_expires_in { get; set; }
    }

    public class SparkUser
    {
        public User user { get; set; }
        public string authorization { get; set; }
        public Token token { get; set; }
    }



}
