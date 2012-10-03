﻿using System.Linq;
using HabrApi.EntityModel;

namespace HabrApi
{
    public class HabrSqlCe
    {
        public void UpsertPost(Post post)
        {
            using (var ctx = HabraStatsEntities.CreateInstance())
            {
                //ctx.ExecuteStoreCommand("DELETE FROM COMMENTS WHERE PostId=" + post.Id);

                foreach (var comment in post.Comments)
                {
                    ctx.Comments.AddObject(comment);
                }
                ctx.SaveChanges();
            }
        }

        public void ClearComments()
        {
            using (var ctx = HabraStatsEntities.CreateInstance())
            {
                ctx.ExecuteStoreCommand("DELETE FROM COMMENTS");
            }
        }

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