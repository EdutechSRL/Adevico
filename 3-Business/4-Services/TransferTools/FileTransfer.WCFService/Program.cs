using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;

namespace FileTransfer.WCFService
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
                        WCFServiceServer service = new WCFServiceServer();
                        service.ServiceStart(args);
                        Console.WriteLine("Press enter to stop WCF service");
                        Console.ReadKey(true);
                        service.ServiceStop();
                        break;
                }
            }
            else
            {

                WCFServiceServer ServiceToRun = new WCFServiceServer();
                ServiceToRun.ServiceName = "FileTransferService";


                ServiceBase.Run(ServiceToRun);
            }


            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[] 
            //{ 
            //    new TransferService() 
            //};
            //ServiceBase.Run(ServicesToRun);
        }
    }
}
