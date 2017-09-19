using System;
using System.Net.Sockets;
using NUnit.Framework;
using ServiceStack.Logging;

namespace ServiceStack.Redis.Tests
{
    [TestFixture]
    public class RedisNativeClientTests
    {
        private const string InvalidHost = "192.168.1.100";
        private const int InvalidPort = 9527;

        [OneTimeSetUp]
        public void TestFixtureSetUp() => LogManager.LogFactory = new ConsoleLogFactory();

        [OneTimeTearDown]
        public void TestFixtureTearDown() => LogManager.LogFactory = new NullLogFactory();

        [Test]
        public void Cannot_connect_Invalid_address()
        {
            try
            {
                var client = new RedisNativeClient(InvalidHost, InvalidPort, db : 1);
            }
            catch (RedisException ex)
            {
                Console.Write(ex.Message);
                var innerEx = ex.InnerException;
                Assert.That(innerEx, Is.TypeOf(typeof(SocketException)));
                Assert.That(((SocketException)innerEx).ErrorCode, Is.EqualTo((int)SocketError.TimedOut));
            }
        }

        [Test]
        public void Cannot_connect_Invalid_address_with_Timeout()
        {
            try
            {
                RedisConfig.DefaultConnectTimeout = 1000;
                var client = new RedisNativeClient(InvalidHost, InvalidPort, db : 1);
            }
            catch (RedisException ex)
            {
                Console.Write(ex.Message);
                var innerEx = ex.InnerException;
                Assert.That(innerEx, Is.TypeOf(typeof(SocketException)));
                Assert.That(((SocketException)innerEx).ErrorCode, Is.EqualTo((int)SocketError.TimedOut));
            }
            finally
            {
                RedisConfig.DefaultConnectTimeout = -1;
            }
        }
    }
}
