using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.IO;
using System.Linq;
using HabrApi;
using HabrApi.EntityModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HabrApiTests
{
    [TestClass]
    public class HabrDbTests
    {
        [TestMethod]
        public void UpsertTest()
        {
            // Run twice to ensure update
            using (var db = HabraStatsEntities.CreateInstance())
            {
                db.UpsertPost(new Habr().DownloadPost(153951));
                db.SaveChanges();
            }
            using (var db = HabraStatsEntities.CreateInstance())
            {
                db.UpsertPost(new Habr().DownloadPost(153951));
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void GenerateAllReportsTest()
        {
            var generator = new StatsGenerator();
            using (var db = HabraStatsEntities.CreateInstance())
            {
                foreach (var report in CommentFilterExtensions.GetAllCommentReports())
                {
                    var query = report.Value(db.Comments).Take(10);
                    var comments = query.ToArray();
                    var reportHtml = generator.GenerateHtmlReport(comments);
                    Assert.IsTrue(reportHtml.Length > 1000);
                }
            }
        }

        [TestMethod]
        [Ignore]
        public void GenerateIndexTest()
        {
            var links = CommentFilterExtensions.GetAllCommentReports()
                .Select(r => string.Format(@"<br/><a href=""{0}.html"">{1}</a>", r.Key.ToWebPageName(), r.Key));
            File.WriteAllText(@"e:\habraIndex.html", links.Aggregate((s1, s2) => s1 + Environment.NewLine + s2));
        }
                
        [TestMethod]
        public void GenerateTestReport()
        {
            using (var db = HabraStatsEntities.CreateInstance())
            {
                var topComments = db.Comments.OrderByDescending(c => c.Score).Take(5);
                var bottomComments = db.Comments.OrderBy(c => c.Score).Take(5);
                var reportHtml = new StatsGenerator().GenerateHtmlReport(topComments.Concat(bottomComments).ToArray());
                Assert.IsFalse(string.IsNullOrEmpty(reportHtml));
                //File.WriteAllText(@"e:\GenerateTestReport.html", reportHtml);
            }
        }

        [TestMethod]
        public void GetTodaysCommentsTest()
        {
            using (var db = HabraStatsEntities.CreateInstance())
            {
                var date = DateTime.Now.AddDays(-1);
                var comments = db.Comments.Where(x => x.Date > date).OrderByDescending(x => x.Score).ToArray();
                Assert.IsTrue(comments.Any());
            }
        }

        [TestMethod]
        public void FirstReportTest()
        {
            var generator = new StatsGenerator();
            using (var db = HabraStatsEntities.CreateInstance())
            {
                foreach (var report in CommentFilterExtensions.GetAllCommentReports().Take(1))
                {
                    var query = report.Value(db.Comments);
                    var comments = query.ToArray();
                    Assert.IsTrue(comments.Any());
                }
            }
        }

        [TestMethod]
        public void TestGetMissingPostIds()
        {
            using (var db = HabraStatsEntities.CreateInstance())
            {
                var ids = db.GetMissingPostIds().ToArray();
                Assert.IsTrue(ids.Any());
            }
        }

    }
}