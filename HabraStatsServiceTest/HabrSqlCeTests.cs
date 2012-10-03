using HabrApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HabrApiTests
{
    [TestClass]
    public class HabrSqlCeTests
    {
        [TestMethod]
        public void Test()
        {
            var habr = new Habr();
            var db = new HabrSqlCe();
            db.UpsertPost(habr.DownloadPost(1));
        }
    }
}