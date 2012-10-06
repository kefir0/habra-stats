using System;
using System.Collections.Generic;
using System.Linq;
using HabrApi;

namespace HabrCacheLoader
{
    internal class Program
    {
        private static void Main()
        {
            //DownloadIntoCache();
            LoadIntoDb();
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
                habr.DownloadPost(id, true);
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
            var db = new HabrSqlCe();
            //db.ClearComments();

            var startTime = DateTime.Now;
            var loadedCount = 0;
            var commentCount = new List<int>();
            foreach (var p in habr.GetCachedPosts(parallelBatchSize: 256))
            {
                db.UpsertPost(p);
                loadedCount++;
                commentCount.Add(p.Comments.Count);
                Console.WriteLine("P/S: {0}; Avg comment count: {1}; id: {2}", (int) (loadedCount/(DateTime.Now - startTime).TotalSeconds), (int) commentCount.Average(), p.Id);
            }
        }
    }
}