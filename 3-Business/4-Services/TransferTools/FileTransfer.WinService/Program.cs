using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;

namespace FileTransfer.WinService
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
                        TransferService service = new TransferService();
                        service.ServiceStart(args);
                        Console.WriteLine("Press enter to stop Win service");
                        Console.ReadKey(true);
                        service.ServiceStop();
                        break;
                }
            }
            else
            {

                TransferService ServiceToRun = new TransferService();
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
