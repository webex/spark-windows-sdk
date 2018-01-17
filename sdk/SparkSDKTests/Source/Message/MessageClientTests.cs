using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkSDK;
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

namespace SparkSDK.Tests
{
    [TestClass()]
    public class MessageClientTests
    {
        private SparkTestFixture fixture;
        private Spark spark;
        private MessageClient messages;
        private string text = "test text.";
        private string fileUrl = "https://developer.ciscospark.com/index.html";
        private TestUser other;
        private Room myRoom;

        [TestInitialize]
        public void SetUp()
        {
            fixture = SparkTestFixture.Instance;
            Assert.IsNotNull(fixture);

            //spark = fixture.spark;
            spark = fixture.CreateSpark();
            Assert.IsNotNull(spark);

            messages = spark.Messages;
            Assert.IsNotNull(messages);

            other = fixture.CreatUser();
            myRoom = fixture.CreateRoom("test room");
        }


        [TestCleanup]
        public void TearDown()
        {
            if (myRoom != null)
            {
                fixture.DeleteRoom(myRoom.Id);
            }
        }

        [TestMethod()]
        public void ListTest()
        {
            var msg = PostMsg(myRoom.Id, null, null, text, null);
            Validate(msg);
            var list = ListMsg(myRoom.Id,null, null,null);
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
        }

        [TestMethod()]
        public void ListByMaxTest()
        {
            PostMsg(myRoom.Id, null, null, text, null);
            PostMsg(myRoom.Id, null, null, text, null);
            PostMsg(myRoom.Id, null, null, text, null);
            var list = ListMsg(myRoom.Id, null, null, 2);
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count == 2);
        }

        [TestMethod()]
        public void ListInvalidRoomIdTest()
        {
            var msg = PostMsg(myRoom.Id, null, null, text, null);
            Validate(msg);
            var list = ListMsg("abc", null, null, null);
            Assert.IsNull(list);
        }

        [TestMethod()]
        public void ListByBeforeTimeTest()
        {
            var msg1 = PostMsg(myRoom.Id, null, null, text, null);
            Assert.IsNotNull(msg1);
            Console.WriteLine("post first Msg at: {0}", DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss.FFFZ"));
            Thread.Sleep(60000);
            var time1 = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss.FFFZ");
            
            Thread.Sleep(60000);
            var msg2 = PostMsg(myRoom.Id, null, null, text, null);
            Assert.IsNotNull(msg2);
            Console.WriteLine("post second Msg at: {0}", DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss.FFFZ"));

            var list = ListMsg(myRoom.Id, time1, null, null);
            Console.WriteLine("list message before {0}", time1);

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
            Assert.IsNotNull(list.Find(item => item.Id == msg1.Id));
            Assert.IsNull(list.Find(item => item.Id == msg2.Id));
        }

        [TestMethod()]
        public void ListByBeforeMessageIdTest()
        {
            var msg1 = PostMsg(myRoom.Id, null, null, "msg1", null);
            var msg2 = PostMsg(myRoom.Id, null, null, "msg2", null);
           

            var list = ListMsg(myRoom.Id, null, msg2.Id, null);
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
            Assert.IsNotNull(list.Find(item => item.Id == msg1.Id));
            Assert.IsNull(list.Find(item => item.Id == msg2.Id));
        }

        [TestMethod()]
        public void ListByBeforeTimeAndMessageIdTest()
        {
            var msg1 = PostMsg(myRoom.Id, null, null, "msg1", null);
            var msg2 = PostMsg(myRoom.Id, null, null, "msg2", null);

            var now = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss.FFFZ");
            var list = ListMsg(myRoom.Id, now, msg2.Id, null);
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
            Assert.IsNotNull(list.Find(item => item.Id == msg1.Id));
            Assert.IsNull(list.Find(item => item.Id == msg2.Id));
        }

        [TestMethod()]
        public void PostToRoomWithTextTest()
        {
            var msg = PostMsg(myRoom.Id, null, null, text, null);
            Validate(msg);
            Assert.AreEqual(text, msg.Text);
        }

        [TestMethod()]
        public void PostToRoomWithFileTest()
        {
            var msg = PostMsg(myRoom.Id, null, null, null, fileUrl);
            Validate(msg);
            Assert.IsNotNull(msg.Files);
        }

        [TestMethod()]
        public void PostToRoomWithTextAndFileTest()
        {
            var msg = PostMsg(myRoom.Id, null, null, text, fileUrl);
            Validate(msg);
            Assert.AreEqual(text, msg.Text);
            Assert.IsNotNull(msg.Files);
        }

        [TestMethod()]
        public void PostToRoomByInvalidIdTest()
        {
            var msg = PostMsg("abc", null, null, text, null);
            Assert.IsNull(msg);
        }

        [TestMethod()]
        public void PostToPersonByIdWithTextTest()
        {
            var msg = PostMsg(null, other.PersonId, null, text, null);
            Validate(msg);
            Assert.AreEqual(text, msg.Text);
        }

        [TestMethod()]
        public void PostToPersonByIdWithFileTest()
        {
            var msg = PostMsg(null, other.PersonId, null, null, fileUrl);
            Validate(msg);
            Assert.IsNotNull(msg.Files);
        }

        [TestMethod()]
        public void PostToPersonByIdWithTextAndFileTest()
        {
            var msg = PostMsg(null, other.PersonId, null, text, fileUrl);
            Validate(msg);
            Assert.AreEqual(text, msg.Text);
            Assert.IsNotNull(msg.Files);
        }

        [TestMethod()]
        public void PostToPersonByInvalidIdTest()
        {
            var msg = PostMsg("abc", null, null, text, null);
            Assert.IsNull(msg);
        }

        [TestMethod()]
        public void PostToPersonByEmailTest()
        {
            var msg = PostMsg(null, null, other.Email, text, null);
            Validate(msg);
            Assert.AreEqual(text, msg.Text);
        }

        [TestMethod()]
        public void PostToPersonByEmailWithFileTest()
        {
            var msg = PostMsg(null, null, other.Email, null, fileUrl);
            Validate(msg);
            Assert.IsNotNull(msg.Files);
        }

        [TestMethod()]
        public void PostToPersonByEmailWithTextAndFileTest()
        {
            var msg = PostMsg(null, null, other.Email, text, fileUrl);
            Validate(msg);
            Assert.AreEqual(text, msg.Text);
            Assert.IsNotNull(msg.Files);
        }

        [TestMethod()]
        public void GetTest()
        {
            var msg = PostMsg(null, null, other.Email, text, null);
            Validate(msg);
            Assert.AreEqual(text, msg.Text);
            var result = GetMsg(msg.Id);
            Validate(result);
            Assert.AreEqual(msg.Id, result.Id);
            Assert.AreEqual(msg.Text, result.Text);
        }

        [TestMethod()]
        public void GetByInvalidIdTest()
        {
            var msg = PostMsg(null, null, other.Email, text, null);
            Validate(msg);
            Assert.AreEqual(text, msg.Text);
            var result = GetMsg("abc");
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            var msg = PostMsg(null, null, other.Email, text, null);
            Validate(msg);
            Assert.AreEqual(text, msg.Text);
            Assert.IsTrue(DeleteMsg(msg.Id));
            Assert.IsNull(GetMsg(msg.Id));
        }

        [TestMethod()]
        public void DeleteByBadIdTest()
        {
            var msg = PostMsg(null, null, other.Email, text, null);
            Validate(msg);
            Assert.AreEqual(text, msg.Text);
            Assert.IsFalse(DeleteMsg("abc"));
            Assert.IsFalse(DeleteMsg(""));
            Assert.IsFalse(DeleteMsg(null));
        }

        private void Validate(Message msg)
        {
            Assert.IsNotNull(msg);
            Assert.IsNotNull(msg.Id);
            Assert.IsNotNull(msg.PersonEmail);
            Assert.IsNotNull(msg.PersonId);
            Assert.IsNotNull(msg.RoomId);
        }

        private Message PostMsg(string roomId, string personId, string email, string text, string file)
        {
            var completion = new ManualResetEvent(false);
            var response = new SparkApiEventArgs<Message>();
            if (roomId != null)
            {
                spark.Messages.PostToRoom(roomId, text, file, rsp =>
                {
                    response = rsp;
                    completion.Set();
                });
            }
            else if (personId != null)
            {
                spark.Messages.PostToPersonByID(personId, text, file, rsp =>
                {
                    response = rsp;
                    completion.Set();
                });
            }
            else if (email != null)
            {
                spark.Messages.PostToPerson(email, text, file, rsp =>
                {
                    response = rsp;
                    completion.Set();
                });
            }
            else
            {
            }


            if (false == completion.WaitOne(30000))
            {
                Console.WriteLine("postMsg outof time");
                return null;
            }

            if (response.IsSuccess == true)
            {
                return response.Data;
            }

            Console.WriteLine($"postMsg fail {response.Error?.ErrorCode} {response.Error?.Reason}");

            return null;
        }

        private Message GetMsg(string msgId)
        {
            var completion = new ManualResetEvent(false);
            var response = new SparkApiEventArgs<Message>();

            spark.Messages.Get(msgId, rsp =>
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

        private List<Message> ListMsg(string roomId, string before = null, string beforeMsg=null, int? max = null)
        {
            var completion = new ManualResetEvent(false);
            var response = new SparkApiEventArgs<List<Message>>();

            spark.Messages.List(roomId, before, beforeMsg, max, rsp =>
            {
                response = rsp;
                completion.Set();
            });
            if (false == completion.WaitOne(30000))
            {
                Console.WriteLine("listMsg outof time");
                return null;
            }

            if (response.IsSuccess == true)
            {
                return response.Data;
            }

            Console.WriteLine($"listMsg faile {response.Error?.ErrorCode} {response.Error?.Reason}");
            return null;
        }

        private bool DeleteMsg(string MsgId)
        {
            var completion = new ManualResetEvent(false);
            var response = new SparkApiEventArgs();

            spark.Messages.Delete(MsgId, rsp =>
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
    }
}