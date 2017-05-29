using lm.Comol.Core.Msmq.Service.Actions.Configurations;
using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace lm.Comol.Core.Msmq.Service.Actions
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Service() 
            };
            ServiceBase.Run(ServicesToRun);
        }
        //static void Main(string[] args)
        //{
        //    if (System.Environment.UserInteractive)
        //    {
        //        string parameter = string.Concat(args);

        //        switch (parameter)
        //        {
        //            case "--install":
        //                ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
        //                Console.ReadKey(true);
        //                break;
        //            case "--uninstall":
        //                ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
        //                Console.ReadKey(true);
        //                break;
        //            default:
        //                Service service = new Service();
        //                service.ServiceStart(args);
        //                Console.WriteLine("Press enter to stop Actions service");
        //                Console.ReadKey(true);
        //                service.ServiceStop();
        //                break;
        //        }
        //    }
        //    else
        //    {

        //        Service ServiceToRun = new Service();
        //        ServiceToRun.ServiceName = Configuration.Cfg.ServiceName;


        //        ServiceBase.Run(ServiceToRun);
        //    }
        //}
    }
}
