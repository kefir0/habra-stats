using System;
using System.Data.Objects;
using System.IO;
using System.Linq;
using HabrApi;
using HabrApi.EntityModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HabrApiTests
{
    [TestClass]
    public class HabrSqlCeTests
    {
        [TestMethod]
        public void UpsertTest()
        {
            // Run twice to ensure update
            using (var db = HabraStatsEntities.CreateInstance())
            {
                db.UpsertPost(new Habr().DownloadPost(1));
            }
            using (var db = HabraStatsEntities.CreateInstance())
            {
                db.UpsertPost(new Habr().DownloadPost(1));
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
                    var fileName = string.Format(@"E:\{0}.html", report.Key);
                    var query = report.Value(db.Comments).Take(10);
                    var queryText = ((ObjectQuery) query).ToTraceString();  // For debugging
                    Assert.IsNotNull(queryText);
                    var comments = query.ToArray();
                    File.WriteAllText(fileName, generator.GenerateCommentStats(comments));
                }
            }
        }

        [TestMethod]
        public void GenerateIndexTest()
        {
            var links = CommentFilterExtensions.GetAllCommentReports()
                .Select(r => string.Format(@"<br/><a href=""{0}.html"">{0}</a>", r.Key.ToWebPageName()));
            File.WriteAllText(@"e:\habraIndex.html", links.Aggregate((s1, s2) => s1 + Environment.NewLine + s2));
        }
    }
}