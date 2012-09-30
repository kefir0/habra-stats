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
            db.InsertPost(habr.DownloadPost(1));
        }
    }
}