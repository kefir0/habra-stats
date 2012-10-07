using System.Linq;
using HabrApi;
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
            Assert.IsTrue(methods.Length == 12);
        }

        [TestMethod]
        public void TestGetAllCombinations()
        {
            var groups = new[] {new[] {"a", "b", "c"}, new[] {"A", "B", "C"}};
            var combinations = CommentFilterExtensions.GetAllCombinations(groups, (s1, s2) => s1 + s2).ToArray();
            Assert.AreEqual(combinations.Length, 9);
            Assert.IsTrue(combinations.SequenceEqual(new[]{"aA", "aB", "aC","bA", "bB", "bC","cA", "cB", "cC",}));
        }

        [TestMethod]
        public void TestGetAllCommentReports()
        {
            var reports = CommentFilterExtensions.GetAllCommentReports().ToArray();
            var count = CommentFilterExtensions.GetCommentReportMethods().GroupBy(m => m.Key.Category).Select(g => g.Count()).Aggregate((c1, c2) => c1*c2);
            Assert.IsTrue(reports.Length == count);
        }
    }
}