using System;
using System.Data.Objects;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using HabrApi;
using HabrApi.EntityModel;
using Timer = System.Timers.Timer;

namespace HabraStatsService
{
    public partial class HabraStatsSvc : ServiceBase
    {
        public const string EventLogSourceName = "HabraStatsSvc";
        private const int HourPeriod = 3; // timer period in hours
        private readonly Timer _timer = new Timer(HourPeriod*60*60*1000);

        public HabraStatsSvc()
        {
            InitializeComponent();
            _timer.Elapsed += OnTimerTick;
        }

        private static void OnTimerTick(object state, ElapsedEventArgs elapsedEventArgs)
        {
            GenerateAndUploadStatsOld();
        }

        protected override void OnStart(string[] args)
        {
            Log("HabraStats started");
            _timer.Start();
            ThreadPool.QueueUserWorkItem(o => GenerateAndUploadStatsSql());
        }

        protected override void OnStop()
        {
            _timer.Stop();
        }

        private static void Log(string message, int eventId = 0, string description = null)
        {
            if (description != null)
                message = message + Environment.NewLine + Environment.NewLine + description;
            EventLog.WriteEntry(EventLogSourceName, message, EventLogEntryType.Information, eventId);
        }

        private static void GenerateAndUploadStatsOld()
        {
            try
            {
                // TODO: We need to sort by comment date, not post date!!!

                var habr = new Habr();
                Log("Loading month posts");
                var monthPosts = habr.GetRecentPosts().TakeWhile(p => p.DaysOld < 35).ToArray();
                Log("Month posts loaded, count = " + monthPosts.Length, 1);

                var statsGenerator = new StatsGenerator();
                Log("Generating daily comment stats");
                var dayReport = statsGenerator.GenerateTopCommentStats(monthPosts.Where(p => p.DaysOld <= 1));
                var twoDaysReport = statsGenerator.GenerateTopCommentStats(monthPosts.Where(p => p.DaysOld <= 2));
                Log("Publishing daily comment stats");
                Uploader.Publish(dayReport, "day.html");
                Uploader.Publish(twoDaysReport, "2days.html");
                Log("Daily comment stats uploaded", 2);

                Log("Generating week comment stats");
                var weekReport = statsGenerator.GenerateTopCommentStats(monthPosts.Where(p => p.DaysOld <= 7));
                Log("Publishing week comment stats");
                Uploader.Publish(weekReport, "week.html");
                Log("Week comment stats uploaded", 2);

                Log("Generating month comment stats");
                var monthReport = statsGenerator.GenerateTopCommentStats(monthPosts.Where(p => p.DaysOld <= 31));
                Log("Publishing month comment stats");
                Uploader.Publish(monthReport, "month.html");
                Log("Month comment stats uploaded", 2);
            }
            catch (Exception e)
            {
                Log("Failed to generate habr stats :( " + e);
            }
        }

        private static void GenerateAndUploadStatsSql()
        {
            try
            {
                var habr = new Habr();
                Log("Loading posts into DB");
                var count = habr.LoadRecentPostsIntoDb();
                Log("Posts loaded into DB: " + count);

                var generator = new StatsGenerator();
                using (var db = HabraStatsEntities.CreateInstance())
                {
                    foreach (var report in CommentFilterExtensions.GetAllCommentReports())
                    {
                        var fileName = string.Format("{0}.html", report.Key.ToWebPageName());
                        var query = report.Value(db.Comments).Take(100);
                        var queryText = ((ObjectQuery) query).ToTraceString();
                        Log("Generating stats: " + report.Key, description: queryText);
                        var comments = query.ToArray();
                        Log("Publishing stats: " + report.Key);
                        Uploader.Publish(generator.GenerateCommentStats(comments), fileName);
                        Log("Stats uploaded: " + report.Key, 2);
                    }
                }

                Log("UPDATE PASS COMPLETE", 3);
            }
            catch (Exception e)
            {
                Log("Failed to generate habr stats :( \n" + e, 13);
            }
        }
    }
}