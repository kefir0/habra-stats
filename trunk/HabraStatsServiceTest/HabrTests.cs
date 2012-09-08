using System;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using HabrApi;
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
            var posts = h.GetRecentPosts().Take(10).OrderByDescending(p => p.Comments.Length).ToArray();
            Assert.IsTrue(posts.Length == 10);
        }

        [TestMethod]
        public void TestSerialization()
        {
            var h = new Habr();
            var post = h.GetRecentPosts().Take(10).OrderByDescending(p => p.Comments.Length).FirstOrDefault();
            var js = new JavaScriptSerializer();
            var json = js.Serialize(post);
            Assert.IsFalse(string.IsNullOrEmpty(json));
        }

        [TestMethod]
        public void TestGenerateCommentStats()
        {
            var h = new Habr();
            var sg = new StatsGenerator();
            var report = sg.GenerateCommentStats(h.GetRecentPosts().TakeWhile(p => p.DaysOld < 7));
            Assert.IsFalse(string.IsNullOrEmpty(report));

            File.WriteAllText(@"e:\HabrCommentsText.html", report);
        }

    }
}