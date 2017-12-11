using System;
using NUnit.Framework;

namespace ServiceStack.Redis.Tests
{
    [TestFixture, Ignore("Integration")]
    public class RedisHyperLogTests
    {
        [Test]
        public void Can_Add_to_Hyperlog()
        {
            var redis = new RedisClient(TestConfig.SingleHost);
            redis.FlushAll();

            redis.AddToHyperLog("hyperlog", "a", "b", "c");
            redis.AddToHyperLog("hyperlog", "c", "d");

            var count = redis.CountHyperLog("hyperlog");

            Assert.That(count, Is.EqualTo(4));

            redis.AddToHyperLog("hyperlog2", "c", "d", "e", "f");

            redis.MergeHyperLogs("hypermerge", "hyperlog", "hyperlog2");

            var mergeCount = redis.CountHyperLog("hypermerge");

            Assert.That(mergeCount, Is.EqualTo(6));
        }
    }
}