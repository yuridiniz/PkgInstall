using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace PkgInstall
{
    class Program
    {
        /// <summary>
        /// Iniciar o serviço, caso o serviço não exista ele cria e iniciará
        /// </summary>
        public static void Start()
        {
            ServiceController servicoDartagnan = new ServiceController("vtb");

            try
            {
                if (servicoDartagnan.Status != ServiceControllerStatus.Running)
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(5000);
                    servicoDartagnan.Start();
                    servicoDartagnan.WaitForStatus(ServiceControllerStatus.Running, timeout);
                }
            }
            catch
            {
                servicoDartagnan.Dispose();
                servicoDartagnan = null;
            }
        }


        static void Main(string[] args)
        {
            InstallManager();
        }

        /// <summary>
        /// Cria o serviço
        /// </summary>
        private static void InstallManager()
        {

            Process processo = new Process();

            processo.StartInfo.RedirectStandardOutput = true;
            processo.StartInfo.CreateNoWindow = true;
            processo.StartInfo.UseShellExecute = false;

            processo.StartInfo.FileName = String.Format("{0}\\vtb.exe", Environment.CurrentDirectory);
            processo.StartInfo.Arguments = "--install";
            processo.Start();

            using (StreamReader reader = processo.StandardOutput)
            {
                string result = reader.ReadToEnd();
                Debug.WriteLine(result);
            }

            Thread.Sleep(3000);

            Start();
        }
    }
}
