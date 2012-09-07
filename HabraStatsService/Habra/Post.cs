using System;

namespace HabraStatsService.Habra
{
    public class Post
    {
        public const string UrlFormat = "http://habrahabr.ru/post/{0}/";

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public Comment[] Comments { get; set; }
        public string Url
        {
            get { return GetUrl(Id); }
        }

        public static string GetUrl(int postId)
        {
            return string.Format(UrlFormat, postId);
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", Id, Title);
        }
    }
}