using System;
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
        private const int CachePostsOlderThanDays = 2;

        public string DownloadString(string url)
        {
            try
            {
                var wc = new WebClient { Encoding = Encoding.UTF8 };
                return wc.DownloadString(url);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Post DownloadPost(int postId)
        {
            var url = Post.GetUrl(postId);
            var fileName = GetCachePath(url);
            if (File.Exists(fileName))
            {
                return Post.Parse(File.ReadAllText(fileName), postId);
            }
            var html = DownloadString(url);
            var post = Post.Parse(html, postId);
            if (post == null || (DateTime.Now - post.Date).TotalDays > CachePostsOlderThanDays)
            {
                File.WriteAllText(fileName, html);
            }
            return post;
        }

        private static string GetCachePath(string postUrl)
        {
            var fileName = postUrl.Replace("/", "-").Replace(".", "-").Replace(":", "-");
            fileName = Path.Combine(CachePath, fileName + ".html");
            return fileName;
        }

        /// <summary>
        /// Enumerates all posts from newest to oldest.
        /// </summary>
        public IEnumerable<Post> GetRecentPosts()
        {
            var lastPostId = GetLastPostId();

            for (var i = lastPostId; i >= 0; i--)
            {
                var post = DownloadPost(i);
                if (post != null)
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
    }
}