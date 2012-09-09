using System.ServiceProcess;

namespace HabraStatsService
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            ServiceBase.Run(new ServiceBase[] {new HabraStatsSvc()});
        }
    }
}