﻿using System.Collections.Generic;
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
        [Ignore]
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
            var posts = GetTestPosts().Take(50).ToArray();
            var report = sg.GenerateTopCommentStats(posts);
            Assert.IsFalse(string.IsNullOrEmpty(report));

            File.WriteAllText(@"e:\HabrCommentsText.html", report);
        }

        [TestMethod]
        public void UploadTest()
        {
            var sg = new StatsGenerator();
            var report = sg.GenerateTopCommentStats(GetTestPosts().Take(50));
            Uploader.Publish(report, "testComments.html");
        }

        [TestMethod]
        [Ignore]
        public void LoadRecentPostsIntoDbTest()
        {
            new Habr().LoadRecentPostsIntoDb();
        }

        [TestMethod]
        public void TestDownloadPost()
        {
            var post = new Habr().DownloadPost(153951);
            Assert.IsNotNull(post);
        }

        private static IEnumerable<Post> GetTestPosts()
        {
            const int maxPostId = 152123;
            var habr = new Habr();
            var count = 0;
            foreach (var cachedPost in habr.GetCachedPosts(maxPostId: maxPostId))
            {
                count++;
                if (count%100 == 0)
                    Debug.WriteLine(count);
                yield return cachedPost;
            }
        }

    }
}