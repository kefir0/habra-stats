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

        private void GenerateAndUploadStatsSql()
        {
            if (_isInProgress)
                return; // Prevent multiple generators
            _isInProgress = true;

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
                        var query = report.Value(db.Comments).Take(50);
                        var queryText = ((ObjectQuery) query).ToTraceString();
                        Log("Generating stats: " + report.Key, description: queryText);
                        var comments = query.ToArray();
                        Uploader.Publish(generator.GenerateCommentStats(comments), fileName);
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
    }
}