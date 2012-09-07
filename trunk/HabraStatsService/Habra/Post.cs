using System;

namespace HabraStatsService.Habra
{
    internal class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public Comment[] Comments { get; set; }
    }
}