using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HabrApi
{
    public class Habr
    {
        private const string RecentPostsUrl = "http://habrahabr.ru/posts/collective/new/";
        private const string CachePath = @"e:\HabrCache";
        private const int CachePostsOlderThanDays = 6;

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
            const int parallelBatchSize = 8;

            for (var i = lastPostId; i >= 0; i-=parallelBatchSize)
            {
                var parallelResults = new ConcurrentBag<Post>();
                Parallel.For(i - parallelBatchSize, i, j =>
                                                           {
                                                               Debug.WriteLine(j);
                                                               var post = DownloadPost(j);
                                                               if (post != null)
                                                                   parallelResults.Add(post);
                                                           });

                foreach (var result in parallelResults)
                {
                    yield return result;
                }
            }
        }

        public int GetLastPostId()
        {
            var lastPostHtml = DownloadString(RecentPostsUrl);
            var lastPostRegex = new Regex(string.Format(Post.UrlFormat, "([0-9]+)"));
            var matches = lastPostRegex.Matches(lastPostHtml);
            var maxId = matches.OfType<Match>().Max(match => int.Parse(match.Groups[1].Value));
            return maxId + 5; // compensate error
        }
    }
}