using System;
using System.Linq;
using System.Web.Script.Serialization;
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
            var posts = h.GetRecentPosts(10).OrderByDescending(p => p.Comments.Length).ToArray();
            Assert.IsTrue(posts.Any());
        }

        [TestMethod]
        public void TestSerialization()
        {
            var h = new Habr();
            var post = h.GetRecentPosts(10).OrderByDescending(p => p.Comments.Length).FirstOrDefault();
            var js = new JavaScriptSerializer();
            var json = js.Serialize(post);
            Assert.IsFalse(string.IsNullOrEmpty(json));
        }
    }
}
