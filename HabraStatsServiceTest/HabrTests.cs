using System;
using System.Linq;
using HabraStatsService.Habra;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HabraStatsServiceTest
{
    [TestClass]
    public class HabrTests
    {
        [TestMethod]
        public void TestGetPosts()
        {
            var h = new Habr();
            var posts = h.GetRecentPosts(10).ToArray();
            Assert.AreEqual(posts.Length, 10);
        }
    }
}
