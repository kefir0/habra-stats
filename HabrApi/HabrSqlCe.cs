using System.Linq;
using EmitMapper;
using Db = HabrApi.EntityModel;

namespace HabrApi
{
    public class HabrSqlCe
    {
        public void InsertPost(Post post)
        {
            // TODO: Extend Db classes and get rid of mapping.
            using (var ctx = new Db.HabraStatsEntities())
            {
                var mapper = GetPostMapper();
                var mappedPost = mapper.Map(post);
                mappedPost.Comments = post.Comments.Select(GetCommentMapper().Map).ToArray();
                ctx.Posts.Add(mappedPost);
                ctx.SaveChanges();
            }
        }

        private static ObjectsMapper<Post, Db.Post> GetPostMapper()
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<Post, Db.Post>();
        }

        private static ObjectsMapper<Comment, Db.Comment> GetCommentMapper()
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<Comment, Db.Comment>();
        }
    }
}