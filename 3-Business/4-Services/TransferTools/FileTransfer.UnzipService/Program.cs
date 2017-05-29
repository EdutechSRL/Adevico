using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;

namespace FileTransfer.UnzipService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (System.Environment.UserInteractive)
            {
                string parameter = string.Concat(args);

                switch (parameter)
                {
                    case "--install":
                        ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                        Console.ReadKey(true);
                        break;
                    case "--uninstall":
                        ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                        Console.ReadKey(true);
                        break;
                    default:
                        FUnzipService service = new FUnzipService();
                        service.ServiceStart(args);
                        Console.WriteLine("Press enter to stop Unzip service");
                        Console.ReadKey(true);
                        service.ServiceStop();
                        break;
                }
            }
            else
            {

                FUnzipService ServiceToRun = new FUnzipService();
                ServiceToRun.ServiceName = "FileTransferService";


                ServiceBase.Run(ServiceToRun);
            }
        }
    }
}
