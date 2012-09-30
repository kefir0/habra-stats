using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HabrApi.EntityModel;

namespace HabrApi
{
    class HabrSqlCe
    {
        private Habr _habr;

        public HabrSqlCe()
        {
            _habr = new Habr();
        }

        public void InsertPost(Post post)
        {
            using (var ctx = new HabraStatsEntities())
            {
                //ctx.Posts.Add(new Post(){Id=post.Id, Comments = post.Comments, Date = post.Date})
            }
        }
    }
}
