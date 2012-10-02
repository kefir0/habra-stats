using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using HabrApi;
using HabrApi.EntityModel;
using HabraStatsService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HabrApiTests
{
    [TestClass]
    public class HabrTests
    {
        [TestMethod]
        public void TestGetPosts()
        {
            var posts = GetTestPosts().Take(100).OrderByDescending(p => p.Comments.Count).ToArray();
            Assert.IsTrue(posts.Length == 100);
        }

        [TestMethod]
        public void TestSerialization()
        {
            var post = GetTestPosts().Take(10).OrderByDescending(p => p.Comments.Count).FirstOrDefault();
            var js = new JavaScriptSerializer();
            var json = js.Serialize(post);
            Assert.IsFalse(string.IsNullOrEmpty(json));
        }

        [TestMethod]
        public void TestGenerateCommentStats()
        {
            var sg = new StatsGenerator();
            var report = sg.GenerateTopCommentStats(GetTestPosts().Take(5));
            Assert.IsFalse(string.IsNullOrEmpty(report));

            File.WriteAllText(@"e:\HabrCommentsText.html", report);
        }

        [TestMethod]
        public void UploadTest()
        {
            var sg = new StatsGenerator();
            var report = sg.GenerateTopCommentStats(GetTestPosts().Take(5));
            Uploader.Publish(report, "testComments.html");
        }

        //[TestMethod]
        //public void GetAllTimeTopCommentsTest()
        //{
        //    var topCommentIds = GetTestPosts()
        //        .SelectMany(p => p.Comments)
        //        .Where(c => c.Score > 30)
        //        .Select(c => new { c.Id, c.Score })
        //        .OrderByDescending(c => c.Score)
        //        .Take(50).ToArray();

        //    foreach (var c in topCommentIds)
        //    {
        //        Console.WriteLine(c);
        //    }
        //}

        private static IEnumerable<Post> GetTestPosts()
        {
            const int maxPostId = 152123;
            var habr = new Habr();
            var count = 0;
            foreach (var cachedPost in habr.GetCachedPosts(maxPostId))
            {
                count++;
                if (count % 100 == 0)
                Debug.WriteLine(count);
                yield return cachedPost;
            }
        }

        //[TestMethod]
        //public void GetAllPostsTest()
        //{
        //    var h = new Habr();
        //    var sg = new StatsGenerator();
        //    var memStart = GC.GetTotalMemory(true)/1024;
        //    var allPosts = h.GetRecentPosts().ToArray();
        //    var memAllPosts = GC.GetTotalMemory(true) / 1024;
        //    Debug.WriteLine("All posts loaded, mem used:" + (memAllPosts - memStart));
        //    var report = sg.GenerateTopCommentStats(allPosts);
        //    var memReport = GC.GetTotalMemory(true) / 1024;
        //    Debug.WriteLine("All posts report generated, mem used:" + (memReport - memStart));
        //    File.WriteAllText(@"e:\HabrCommentsALL.html", report);
        //}

    }
}