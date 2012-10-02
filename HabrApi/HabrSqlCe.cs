using HabrApi.EntityModel;

namespace HabrApi
{
    public class HabrSqlCe
    {
        public void InsertPost(Post post)
        {
            using (var ctx = new HabraStatsEntities())
            {
                foreach (var comment in post.Comments)
                {
                    ctx.Comments.AddObject(comment);
                }
                ctx.SaveChanges();
            }
        }
    }
}