using HabrApi;
using HabrApi.EntityModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HabrApiTests
{
    [TestClass]
    public class HabrSqlCeTests
    {
        [TestMethod]
        public void Test()
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
    }
}