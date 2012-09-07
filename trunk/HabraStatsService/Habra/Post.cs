using System;

namespace HabraStatsService.Habra
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public Comment[] Comments { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", Id, Title);
        }
    }
}