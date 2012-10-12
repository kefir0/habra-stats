using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using HabrApi.EntityModel;

namespace HabrApi
{
    public class Habr
    {
        private const string RecentPostsUrl = "http://habrahabr.ru/posts/collective/new/";
        private const string CachePath = @"e:\HabrCache";
        private const int CachePostsOlderThanDays = 3;

        public string DownloadString(string url)
        {
            while (true)
            {
                try
                {
                    var wc = new WebClient { Encoding = Encoding.UTF8 };
                    Console.WriteLine("Downloading " + url);
                    var result = wc.DownloadString(url);
                    if (!string.IsNullOrWhiteSpace(result))
                        return result;
                    Console.WriteLine("Url {0} returned empty result", url);
                }
                catch (WebException webEx)
                {
                    if (webEx.Status == WebExceptionStatus.ProtocolError)
                    {
                        // 404, etc - return content
                        var sr = new StreamReader(webEx.Response.GetResponseStream());
                        return sr.ReadToEnd();
                    }
                    Console.WriteLine(webEx);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                Thread.Sleep(500);
            }
        }

        public Post DownloadPost(int postId, bool skipComments = false, bool ignoreCache = false)
        {
            var url = Post.GetUrl(postId);
            var fileName = GetCachePath(url);
            if (!ignoreCache && File.Exists(fileName))
            {
                // File exists: check if it is valid, parse to retrive post date
                // And determine whether this post can be loaded from cache
                var cachedHtml = File.ReadAllText(fileName);
                if (!string.IsNullOrWhiteSpace(cachedHtml))
                {
                    var cachedPost = Post.Parse(cachedHtml, postId, skipComments);
                    if (ShouldCache(cachedPost))
                    {
                        Console.WriteLine("Returning from cache: " + postId);
                        return cachedPost;
                    }
                }
            }
            var html = DownloadString(url);
            var post = Post.Parse(html, postId, skipComments);
            if (ShouldCache(post))
            {
                File.WriteAllText(fileName, html);
            }
            return post;
        }

        public bool IsInCache(int postId)
        {
            var url = Post.GetUrl(postId);
            var fileName = GetCachePath(url);
            return File.Exists(fileName) && new FileInfo(fileName).Length > 10;
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
        public IEnumerable<Post> GetRecentPosts(bool ignoreCache = false)
        {
            var lastPostId = GetLastPostId();

            for (var i = lastPostId; i >= 0; i--)
            {
                var post = DownloadPost(i, ignoreCache: ignoreCache);
                if (post != null)
                    yield return post;
            }
        }

        /// <summary>
        /// Enumerates all valid posts in cache.
        /// </summary>
        public IEnumerable<Post> GetCachedPosts(int startPostId = 0, int? maxPostId = null)
        {
            var lastPostId = maxPostId ?? GetLastPostId();

            for (var i = startPostId; i <= lastPostId; i++)
            {
                if (!IsInCache(i)) continue;
                var cachePath = GetCachePath(Post.GetUrl(i));
                var post = Post.Parse(File.ReadAllText(cachePath), i);
                if (post != null)
                    yield return post;
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

        public int LoadRecentPostsIntoDb()
        {
            var count = 0;
            // TODO: Look like there is a bug with nonexistent posts getting into the disk cache.....!
            foreach (var post in GetRecentPosts(ignoreCache: true).TakeWhile(post => !ShouldCache(post)))
            {
                using (var db = HabraStatsEntities.CreateInstance())
                {
                    db.UpsertPost(post);
                    db.SaveChanges();
                    count++;
                }
            }
            return count;
        }

    }
}