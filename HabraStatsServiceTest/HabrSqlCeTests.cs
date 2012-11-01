﻿using System;
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
                    var fileName = string.Format(@"E:\{0}.html", report.Key);
                    var query = report.Value(db.Comments).Take(10);
                    var queryText = ((ObjectQuery) query).ToTraceString();  // For debugging
                    Assert.IsNotNull(queryText);
                    var comments = query.ToArray();
                    File.WriteAllText(fileName, generator.GenerateHtmlReport(comments));
                }
            }
        }

        [TestMethod]
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
                File.WriteAllText(@"e:\GenerateTestReport.html", reportHtml);
            }
            
        }

    }
}