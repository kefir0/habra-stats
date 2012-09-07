using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace HabraStatsService.Habra
{
    public class Habr
    {
        private const string RecentPostsUrl = "http://habrahabr.ru/posts/collective/new/";
        private const string CachePath = @"e:\HabrCache";

        private static readonly Regex CommentRegex =
            new Regex(
                "<div class=\"comment_item\" id=\"(.*?)\".*?<span class=\"score\".*?>(.*?)</span>.*?<div class=\"message.*?\">(.*?)</div>",
                RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex TitleRegex = new Regex("<title>(.*?)</title>", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // 			<time datetime="2012-08-31T23:23:16+04:00">31 августа 2012 в 23:23</time>
        private static readonly Regex DateRegex = new Regex("TODO", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);


        public string DownloadString(string url)
        {
            var fileName = GetCachePath(url);
            if (File.Exists(fileName))
            {
                return File.ReadAllText(fileName);
            }

            var html = "";
            var wc = new WebClient {Encoding = Encoding.UTF8};
            try
            {
                html = wc.DownloadString(url);
            }
            catch
            {
            }

            File.WriteAllText(fileName, html);
            return html;
        }

        public string DownloadPost(int postId)
        {
            return DownloadString(Post.GetUrl(postId));
        }

        private static string GetCachePath(string postUrl)
        {
            var fileName = postUrl.Replace("/", "-").Replace(".", "-").Replace(":", "-");
            fileName = Path.Combine(CachePath, fileName + ".html");
            return fileName;
        }

        private static int ParseCommentRating(string commentRating)
        {
            return int.Parse(commentRating.Replace("–", "-"));
        }

        public IEnumerable<Post> GetRecentPosts(int postCount)
        {
            var lastPostId = GetLastPostId();

            for (var i = lastPostId; i > lastPostId - postCount; i--)
            {
                var postHtml = DownloadPost(i);
                var comments = GetComments(postHtml, i);
                var title = TitleRegex.Match(postHtml).Groups[1].Value;
                var post = new Post
                               {
                                   Id = i,
                                   Title = title,
                                   Comments = comments.ToArray()
                               };
                yield return post;
            }
        }

        private int GetLastPostId()
        {
            var lastPostHtml = DownloadString(RecentPostsUrl);
            var lastPostRegex = new Regex(string.Format(Post.UrlFormat, "([0-9]+)"));
            var match = lastPostRegex.Match(lastPostHtml);
            var lastPostId = int.Parse(match.Groups[1].Value);
            return lastPostId;
        }

        private static IEnumerable<Comment> GetComments(string postHtml, int i)
        {
            return CommentRegex.Matches(postHtml).OfType<Match>()
                .Select(c =>
                        new Comment
                            {
                                Id = c.Groups[1].Value,
                                Score = ParseCommentRating(c.Groups[2].Value),
                                Text = c.Groups[3].Value.Trim(),
                                Url = GetCommentUrl(i, c.Groups[1].Value)
                            });
        }

        private static string GetCommentUrl(int postId, string commentId)
        {
            return string.Format(Post.UrlFormat, postId).TrimEnd('/') + "#" + commentId;
        }
    }
}