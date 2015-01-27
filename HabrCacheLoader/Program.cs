using System;
using System.Collections.Generic;
using System.Linq;
using HabrApi;
using HabrApi.EntityModel;

namespace HabrCacheLoader
{
    internal class Program
    {
        private static void Main()
        {
            //DownloadIntoCache();
            //LoadIntoDb();
            DownloadIntoCacheAndDb();
        }

        private static void DownloadIntoCache()
        {
            var habr = new Habr();
            var lastPostId = habr.GetLastPostId();
            var startTime = DateTime.Now;
            var loadedCount = 0;
            var notCachedPosts = Enumerable.Range(1, lastPostId).Where(i => !habr.IsInCache(i)).ToArray();
            Console.WriteLine("Posts to load: " + notCachedPosts.Length);
            foreach (var id in notCachedPosts)
            {
                habr.DownloadPost(id, skipComments: true);
                loadedCount++;
                var runTime = DateTime.Now - startTime;
                var postsPerSecond = (loadedCount)/runTime.TotalSeconds;
                var eta = TimeSpan.FromSeconds((notCachedPosts.Length - loadedCount)/postsPerSecond);
                Console.WriteLine("[{2}] {0:dd\\.hh\\:mm\\:ss}; P/s: {1:0.0}; ETA: {3:dd\\.hh\\:mm\\:ss}; {4} of {5}", runTime, postsPerSecond, id, eta, loadedCount, notCachedPosts.Length);
            }
        }

        private static void DownloadIntoCacheAndDb()
        {
            var habr = new Habr();
            var lastPostId = habr.GetLastPostId();
            var startTime = DateTime.Now;
            var loadedCount = 0;
            const int startPostId = 243997;
            var notCachedPosts = Enumerable.Range(startPostId, lastPostId - startPostId)
                //.Where(i => !habr.IsInCache(i))
                .ToArray();
            Console.WriteLine("Posts to load: " + notCachedPosts.Length);
            foreach (var id in notCachedPosts)
            {
                var post = habr.DownloadPost(id, skipComments: false, ignoreCache:true);

                if (post != null)
                {
                    using (var db = new HabraStatsEntities())
                    {
                        db.UpsertPost(post);
                        db.SaveChanges();
                    }
                }

                loadedCount++;
                var runTime = DateTime.Now - startTime;
                var postsPerSecond = (loadedCount)/runTime.TotalSeconds;
                var eta = TimeSpan.FromSeconds((notCachedPosts.Length - loadedCount)/postsPerSecond);
                Console.WriteLine("[{2}] {0:dd\\.hh\\:mm\\:ss}; P/s: {1:0.0}; ETA: {3:dd\\.hh\\:mm\\:ss}; {4} of {5}", runTime, postsPerSecond, id, eta, loadedCount, notCachedPosts.Length);
            }
        }

        private static void LoadIntoDb()
        {
            var habr = new Habr();
            var startTime = DateTime.Now;
            var loadedCount = 0;
            var commentCount = new List<int>();
            var startPost = GetMaxSqlPostId() - 200;

            foreach (var p in habr.GetCachedPosts(startPost - 1))
            {
                loadedCount++;
                commentCount.Add(p.Comments.Count);
                using (var db = new HabraStatsEntities())
                {
                    db.UpsertPost(p);
                    db.SaveChanges();
                }
                Console.WriteLine("P/S: {0}; Avg comment count: {1}; id: {2}", (int) (loadedCount/(DateTime.Now - startTime).TotalSeconds), (int) commentCount.Average(), p.Id);
            }
        }

        private static int GetMaxSqlPostId()
        {
            using (var db = new HabraStatsEntities())
            {
                return db.Posts.Max(p => p.Id);
            }
        }
    }
}