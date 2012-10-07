using System.Linq;
using HabrApi;
using HabrApi.EntityModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HabrApiTests
{
    [TestClass]
    public class CommentFilterExtensionsTests
    {
        [TestMethod]
        public void TestGetCommentReportMethods()
        {
            var methods = CommentFilterExtensions.GetCommentReportMethods().ToArray();
            Assert.AreEqual(methods.Length,  13);
        }

        [TestMethod]
        public void TestGetAllCombinations()
        {
            var groups = new[] {new[] {"a", "b", "c"}, new[] {"A", "B", "C"}};
            var combinations = CommentFilterExtensions.GetAllCombinations(groups, (s1, s2) => s1 + s2).ToArray();
            Assert.AreEqual(combinations.Length, 9);
            Assert.IsTrue(combinations.SequenceEqual(new[]{"aA", "aB", "aC","bA", "bB", "bC","cA", "cB", "cC"}));
        }

        [TestMethod]
        public void TestGetAllCommentReports()
        {
            var reports = CommentFilterExtensions.GetAllCommentReports().ToArray();
            var count = CommentFilterExtensions.GetCommentReportMethods().GroupBy(m => m.Key.Category).Select(g => g.Count()).Aggregate((c1, c2) => c1*c2);
            Assert.IsTrue(reports.Length == count);
        }

        [TestMethod]
        public void TestGetAllCommentReportsFunctions()
        {
            var reports = CommentFilterExtensions.GetAllCommentReports().ToArray();
            foreach (var pair in reports)
            {
                var count = pair.Value(TestQueryable()).Count();
                Assert.IsTrue(count == 0 || count == 2);
            }
        }

        private static IQueryable<Comment> TestQueryable()
        {
            return new[] {new Comment {Text = "Tst"}, new Comment {Text = "Tst"}}.AsQueryable();
        }
    }
}