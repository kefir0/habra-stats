using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace HabraStatsService
{
    [RunInstaller(true)]
    public class HabraStatsSvcInstaller : Installer
    {
        public HabraStatsSvcInstaller()
        {
            var installer = new ServiceInstaller();
            var installer2 = new ServiceProcessInstaller();
            installer.ServiceName = "HabrStatsPublisher";
            installer.DisplayName = "Habrahabr Stats Publisher";
            installer.Description = "This service gathers and publishes Habrahabr.ru statistics to the web.";
            Installers.Add(installer);
            installer2.Account = ServiceAccount.LocalSystem;
            installer2.Password = null;
            installer2.Username = null;
            Installers.Add(installer2);
        }
    }
}