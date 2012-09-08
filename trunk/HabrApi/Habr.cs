using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace HabrApi
{
    public class Habr
    {
        private const string RecentPostsUrl = "http://habrahabr.ru/posts/collective/new/";
        private const string CachePath = @"e:\HabrCache";
        private const int CachePostsOlderThanDays = 4;

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
                var cachedPost = Post.Parse(File.ReadAllText(fileName), postId);
                if (ShouldCache(cachedPost))
                    return cachedPost;
            }
            var html = DownloadString(url);
            var post = Post.Parse(html, postId);
            if (ShouldCache(post))
            {
                File.WriteAllText(fileName, html);
            }
            return post;
        }

        private static bool ShouldCache(Post post)
        {
            return post == null || post.DaysOld > CachePostsOlderThanDays;
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

        public int GetLastPostId()
        {
            var lastPostHtml = DownloadString(RecentPostsUrl);
            var lastPostRegex = new Regex(string.Format(Post.UrlFormat, "([0-9]+)"));
            var match = lastPostRegex.Match(lastPostHtml);
            var lastPostId = int.Parse(match.Groups[1].Value);
            return lastPostId;
        }
    }
}