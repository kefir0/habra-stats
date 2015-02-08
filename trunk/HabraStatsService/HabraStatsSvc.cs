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
        private bool _isInProgress;

        public HabraStatsSvc()
        {
            InitializeComponent();
            _timer.Elapsed += OnTimerTick;
        }

        private void OnTimerTick(object state, ElapsedEventArgs elapsedEventArgs)
        {
            GenerateAndUploadStatsSql();
        }

        protected override void OnStart(string[] args)
        {
            Log("HabraStats started");
            _timer.Start();
            ThreadPool.QueueUserWorkItem(o =>
                                             {
                                                 GenerateAndUploadStatsSql(downloadPosts: false);
                                                 GenerateAndUploadStatsSql();
                                             });
        }

        protected override void OnStop()
        {
            _timer.Stop();
        }

        public static void Log(string message, int eventId = 0, string description = null)
        {
            if (description != null)
                message = message + Environment.NewLine + Environment.NewLine + description;
            EventLog.WriteEntry(EventLogSourceName, message, EventLogEntryType.Information, eventId);
        }

        private void GenerateAndUploadStatsSql(bool downloadPosts = true)
        {
            if (_isInProgress)
                return; // Prevent multiple generators
            _isInProgress = true;

            try
            {
                if (downloadPosts)
                {
                    Log("Loading posts into DB");
                    var sw = Stopwatch.StartNew();
                    var count = Habr.LoadRecentPostsIntoDb();
                    var elapsed = sw.Elapsed;
                    Log(string.Format("Posts loaded into DB: {0}; Elapsed: {1}; Posts/second: {2}", count, elapsed, count / elapsed.TotalSeconds));
                }

                var generator = new StatsGenerator();
                var first = true;
                using (var db = HabraStatsEntities.CreateInstance())
                {
                    foreach (var report in CommentFilterExtensions.GetAllCommentReports())
                    {
                        var fileName = string.Format("{0}.html", report.Key.ToWebPageName());
                        var query = report.Value(db.Comments).Take(50);
                        var queryText = query is ObjectQuery ? ((ObjectQuery) query).ToTraceString() : string.Empty;
                        Log("Generating stats: " + report.Key, description: queryText);
                        var comments = query.ToArray();
                        var htmlReport = generator.GenerateHtmlReport(comments, report.Key);
                        Uploader.Publish(htmlReport, fileName);

                        if (first)
                        {
                            first = false;
                            Uploader.Publish(htmlReport, "index.html");
                        }
                    }
                }

                Log("UPDATE PASS COMPLETE", 3);
            }
            catch (Exception e)
            {
                Log("Failed to generate habr stats :( \n" + e, 13);
            }
            finally
            {
                _isInProgress = false;
            }
        }

        private void GenerateAndUploadRss()
        {
            // TODO: Find top 10 comments of the previous day or something
            //System.ServiceModel.Syndication.SyndicationItem
            //SyndicationItem
        }

        private void GenerateAndUploadStats()
        {
            // ЗА ПЕРИОД
            // Всего комментариев
            // Положительных, отрицательных
            // Средний рейтинг
        }
    }
}