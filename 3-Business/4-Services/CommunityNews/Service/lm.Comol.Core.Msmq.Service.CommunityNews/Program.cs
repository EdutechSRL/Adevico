using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace lm.Comol.Core.Msmq.Service.CommunityNews
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
				new CommunityNewsService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
