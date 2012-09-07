using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace HabraStatsService
{
    public partial class HabraStatsSvc : ServiceBase
    {
        private const int HourPeriod = 3;  // timer period in hours
        private Timer _timer = new Timer(HourPeriod*60*60*1000); 

        private static void OnTimerTick(object state)
        {
            // TODO: Run job
        }

        public HabraStatsSvc()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
