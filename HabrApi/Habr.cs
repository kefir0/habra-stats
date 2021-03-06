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
    public static class Habr
    {
        private const string CachePath = @"f:\HabrCache";
        private const double CachePostsOlderThanDays = 6;

        public static string DownloadString(string url)
        {
            while (true)
            {
                try
                {
                    var wc = new WebClientEx { Encoding = Encoding.UTF8 };
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

        public static Post DownloadPost(int postId, bool skipComments = false, bool ignoreCache = false)
        {
            return Site.Instances.Select(site => DownloadPost(postId, site, skipComments, ignoreCache)).FirstOrDefault(post => post != null);
        }

        public static Post DownloadPost(int postId, Site site, bool skipComments = false, bool ignoreCache = false)
        {
            if (site == null)
            {
                throw new ArgumentNullException("site");
            }

            var url = site.GetUrl(postId);
            var fileName = GetCachePath(url);
            if (!ignoreCache && File.Exists(fileName))
            {
                // File exists: check if it is valid, parse to retrieve post date
                // And determine whether this post can be loaded from cache
                var cachedHtml = File.ReadAllText(fileName);
                if (!string.IsNullOrWhiteSpace(cachedHtml))
                {
                    var cachedPost = Post.Parse(cachedHtml, postId, site, skipComments);
                    if (ShouldCache(cachedPost))
                    {
                        Console.WriteLine("Returning from cache: " + postId);
                        return cachedPost;
                    }
                }
            }
            var html = DownloadString(url);
            var post = Post.Parse(html, postId, site, skipComments);
            if (ShouldCache(post))
            {
                File.WriteAllText(fileName, html);
            }
            return post;
        }

        public static bool IsInCache(int postId)
        {
            return Site.Instances.Select(x => x.GetUrl(postId)).Any(url =>
            {
                var fileName = GetCachePath(url);
                return File.Exists(fileName) && new FileInfo(fileName).Length > 10;
            });
        }

        private static bool ShouldCache(Post post)
        {
            return post != null && post.DaysOld > CachePostsOlderThanDays;
        }

        private static string GetCachePath(string postUrl)
        {
            var fileName = postUrl.Replace("/", "-").Replace(".", "-").Replace(":", "-") + "-";
            fileName = Path.Combine(CachePath, fileName + ".html");
            return fileName;
        }

        /// <summary>
        /// Enumerates all valid posts in cache.
        /// </summary>
        public static IEnumerable<Post> GetCachedPosts(int startPostId = 0, int? maxPostId = null)
        {
            var lastPostId = maxPostId ?? GetLastPostId();

            for (var i = startPostId; i <= lastPostId; i++)
            {
                foreach (var site in Site.Instances)
                {
                    var cachePath = GetCachePath(site.GetUrl(i));
                    if (File.Exists(cachePath))
                    {
                        var post = Post.Parse(File.ReadAllText(cachePath), i, site);
                        if (post != null)
                            yield return post;
                    }
                }
            }
        }

        public static int GetLastPostId(Site site = null)
        {
            site = site ?? Site.Instances.First();
            var lastPostHtml = DownloadString(site.Url);
            var lastPostRegex = new Regex(string.Format(Site.UrlFormat, site.Url, "([0-9]+)"));
            var matches = lastPostRegex.Matches(lastPostHtml);
            var maxId = matches.OfType<Match>().Max(match => int.Parse(match.Groups[1].Value));
            return maxId + 5; // compensate error
        }

        public static int LoadRecentPostsIntoDb()
        {
            var count = 0;
            foreach (var site in Site.Instances)
            {
                foreach (var post in GetRecentPosts(site, true).TakeWhile(post => post.DaysOld < CachePostsOlderThanDays))
                {
                    using (var db = HabraStatsEntities.CreateInstance())
                    {
                        db.UpsertPost(post);
                        db.SaveChanges();
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Enumerates all posts from newest to oldest.
        /// </summary>
        private static IEnumerable<Post> GetRecentPosts(Site site, bool ignoreCache = false)
        {
            var lastPostId = GetLastPostId(site);

            for (var i = lastPostId; i >= 0; i--)
            {
                var post = DownloadPost(i, site, ignoreCache: ignoreCache);
                if (post != null)
                    yield return post;
            }
        }
    }
}