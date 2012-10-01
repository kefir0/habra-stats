using HabrApi.EntityModel;

namespace HabrApi
{
    public class HabrSqlCe
    {
        public void InsertPost(Post post)
        {
            // TODO: Extend Db classes and get rid of mapping.
            using (var ctx = new HabraStatsEntities1())
            {
                ctx.Posts.AddObject(post);
                ctx.SaveChanges();
            }
        }
    }
}