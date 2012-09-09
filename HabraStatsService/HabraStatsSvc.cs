﻿using System;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using HabrApi;
using System.Diagnostics;

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
            GenerateAndUploadStats();
        }

        protected override void OnStart(string[] args)
        {
            Log("HabraStats started");
            _timer.Start();
            GenerateAndUploadStats();
        }

        protected override void OnStop()
        {
            _timer.Stop();
        }

        private static void Log(string message)
        {
            EventLog.WriteEntry(EventLogSourceName, message);
        }

        private static void GenerateAndUploadStats()
        {
            var habr = new Habr();
            var statsGenerator = new StatsGenerator();

            Log("Generating daily comment stats");
            var dayReport = statsGenerator.GenerateTopCommentStats(habr.GetRecentPosts()
                                                                       .TakeWhile(p => (DateTime.Now.Date - p.Date.Date).TotalDays <= 1)
                                                                       .Where(p => p.Date.Date == DateTime.Now.Date));
            
            Log("Publishing daily comment stats");
            Uploader.Publish(dayReport, "day.html");
            Log("Daily comment stats uploaded");


            Log("Generating week comment stats");
            var weekReport = statsGenerator.GenerateTopCommentStats(habr.GetRecentPosts().TakeWhile(p => p.DaysOld < 8));
            Log("Publishing week comment stats");
            Uploader.Publish(weekReport, "week.html");
            Log("Week comment stats uploaded");
        }
    }
}