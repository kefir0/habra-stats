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
            
            // Run twice to ensure update
            db.UpsertPost(habr.DownloadPost(100006));
            db.UpsertPost(habr.DownloadPost(100006));
        }
    }
}