using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HabrApi.EntityModel
{
    public partial class HabraStatsEntities
    {
        public const string DefaultConnectionString =
            @"metadata=res://*/EntityModel.HabraStats.csdl|res://*/EntityModel.HabraStats.ssdl|res://*/EntityModel.HabraStats.msl;provider=System.Data.SqlClient;provider connection string="";data source=NAXAH-PC\SQLEXPRESS;initial catalog=habrastats;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework""";

        public static HabraStatsEntities CreateInstance()
        {
            return new HabraStatsEntities(DefaultConnectionString);
        }

        public void UpsertPost(Post post)
        {
            ExecuteStoreCommand("DELETE FROM POSTS WHERE ID=" + post.Id);
            
            var commentIds = post.Comments.Select(x => x.Id.ToString(CultureInfo.InvariantCulture)).ToList();
            if (commentIds.Any())
            {
                var commentIdsString = commentIds.Aggregate((x, y) => string.Format("{0}, {1}", x, y));
                ExecuteStoreCommand("DELETE FROM COMMENTS WHERE ID IN (" + commentIdsString + ")");
            }

            Posts.AddObject(post);
        }

        partial void OnContextCreated()
        {
            CommandTimeout = 60*5;
        }

        public IEnumerable<int> GetMissingPostIds(int minPostId = 0, int? maxPostId = null)
        {
            var ids = new HashSet<int>(Posts.Select(x => x.Id));
            maxPostId = maxPostId ?? (ids.Any() ? ids.Max() : -1);
            for (int i = minPostId; i <= maxPostId; i++)
            {
                if (!ids.Contains(i))
                {
                    yield return i;
                }
            }
        }
    }
}