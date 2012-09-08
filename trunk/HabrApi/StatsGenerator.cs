using System.Collections.Generic;
using System.Linq;

namespace HabrApi
{
    public class StatsGenerator
    {
        public string GenerateCommentStats(IEnumerable<Post> posts)
        {
            // TODO: Xslt
            return posts.SelectMany(p => p.Comments).OrderByDescending(c => c.Score)
                .Take(100).ToArray().ToXml();
        }
    }
}