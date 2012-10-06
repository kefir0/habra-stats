using System.Linq;
using HabrApi.EntityModel;

namespace HabrApi
{
    public class HabrSqlCe
    {
        public void UpsertPost(Post post)
        {
            using (var ctx = HabraStatsEntities.CreateInstance())
            {
                ctx.ExecuteStoreCommand("DELETE FROM POSTS WHERE ID=" + post.Id);
                ctx.Posts.AddObject(post);
                ctx.SaveChanges();
                
                DetachPost(post, ctx);
            }
        }

        private static void DetachPost(Post post, HabraStatsEntities ctx)
        {
            // TODO: Think...
            var comments = post.Comments.ToArray();
            foreach (var comment in comments)
            {
                ctx.Detach(comment);
            }
            ctx.Detach(post);
            foreach (var comment in comments)
            {
                post.Comments.Add(comment);
            }
        }

        //public void ClearComments()
        //{
        //    using (var ctx = HabraStatsEntities.CreateInstance())
        //    {
        //        ctx.ExecuteStoreCommand("DELETE FROM COMMENTS");
        //    }
        //}

        //private static void UpsertComment(HabraStatsEntities ctx, Comment comment)
        //{
        //    if (ctx.Comments.SingleOrDefault(c => c.Id == comment.Id) != null)
        //    {
        //        ctx.Comments.ApplyCurrentValues(comment);
        //    }
        //    else
        //    {
        //        ctx.Comments.AddObject(comment);
        //    }
        //}
    }
}