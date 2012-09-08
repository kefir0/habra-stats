using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace HabrApi
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
            return String.Format(UrlFormat, postId);
        }

        public override string ToString()
        {
            return String.Format("[{0}] {1}", Id, Title);
        }

        public static Post Parse(string html, int id)
        {
            if (string.IsNullOrEmpty(html))
                return null;

            var comments = Comment.Parse(html, id);
            var title = TitleRegex.Match(html).Groups[1].Value;
            var date = DateTime.Parse(DateRegex.Match(html).Groups[1].Value.Replace(" � ", " "));
            return new Post
            {
                Id = id,
                Title = title,
                Date = date,
                Comments = comments.ToArray()
            };
        }

        private static readonly Regex TitleRegex = new Regex("<title>(.*?)</title>", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex DateRegex = new Regex("<div class=\"published\">(.*?)</div>", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}