using System;
using System.Threading;
using System.Threading.Tasks;
using HabrApi;

namespace HabrCacheLoader
{
    internal class Program
    {
        private static void Main()
        {
            var habr = new Habr();
            var lastPostId = habr.GetLastPostId();
            var startTime = DateTime.Now;
            var loadedCount = 0;
            Parallel.For(0, lastPostId, new ParallelOptions {MaxDegreeOfParallelism = 32},
                         id =>
                             {
                                 habr.DownloadPost(id);
                                 Interlocked.Increment(ref loadedCount);
                                 var runTime = DateTime.Now - startTime;
                                 var postsPerSecond = (loadedCount)/runTime.TotalSeconds;
                                 var eta = TimeSpan.FromSeconds(lastPostId/postsPerSecond);
                                 Console.WriteLine("Running: {0:hh\\:mm\\:ss}; Posts/second: {1:0.0}; Id: {2}; ETA: {3:hh\\:mm\\:ss}", runTime, postsPerSecond, id, eta);
                             });
        }
    }
}