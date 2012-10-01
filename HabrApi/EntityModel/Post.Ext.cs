using System;
using System.Data.Objects.DataClasses;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace HabrApi.EntityModel
{
    public partial class Post
    {
        public const string UrlFormat = "http://habrahabr.ru/post/{0}/";
        private static readonly Regex TitleRegex = new Regex("<title>(.*?)</title>", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex DateRegex = new Regex("<div class=\"published\">(.*?)</div>", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Url
        {
            get { return GetUrl(Id); }
        }

        public double DaysOld
        {
            get { return (DateTime.Now - Date).TotalDays; }
        }

        public static string GetUrl(int postId)
        {
            return String.Format(UrlFormat, postId);
        }

        public override string ToString()
        {
            return String.Format("[{0}] {1}", Id, Title);
        }

        public static Post Parse(string html, int id, bool skipComments = false)
        {
            if (string.IsNullOrEmpty(html))
                return null;

            try
            {
                var title = TitleRegex.Match(html).Groups[1].Value;
                var date = DateTime.Parse(DateRegex.Match(html).Groups[1].Value.Replace(" в ", " "), CultureInfo.GetCultureInfo("ru-RU"));
                var post = new Post
                               {
                                   Id = id,
                                   Title = title,
                                   Date = date
                               };

                if (!skipComments)
                {
                    foreach (var comment in Comment.Parse(html, post))
                    {
                        post.Comments.Add(comment);
                    }
                }

                return post;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}