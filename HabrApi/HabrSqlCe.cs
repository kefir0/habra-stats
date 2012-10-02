using HabrApi.EntityModel;

namespace HabrApi
{
    public class HabrSqlCe
    {
        public void InsertPost(Post post)
        {
            using (var ctx = HabraStatsEntities.CreateInstance())
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