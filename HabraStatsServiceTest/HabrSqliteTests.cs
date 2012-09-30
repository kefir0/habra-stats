using HabrApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HabrApiTests
{
    [TestClass]
    public class HabrSqliteTests
    {
        [TestMethod]
        public void Test()
        {
            var h = new HabrSqlite();
            h.LoadIntoSqlite(1);
        }
    }
}