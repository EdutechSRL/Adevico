using lm.Comol.Core.Msmq.Services.Mail.Service.Config;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace lm.Comol.Core.Msmq.Services.Mail.WinSrvHost
{
    [RunInstaller(true)]
    public partial class MailServiceInstaller : System.Configuration.Install.Installer
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public MailServiceInstaller()
        {
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service = new ServiceInstaller();

            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            Configuration cfg = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            String path = cfg.AppSettings.Settings["ServicePath"].Value;
            ServiceConfig config = lm.Comol.Core.WinService.Configurations.Configuration<ServiceConfig>.Load(path);;
      
            service.ServiceName = config.Service.ServiceName;
  
            service.StartType = ServiceStartMode.Automatic;
            Installers.Add(process);
            Installers.Add(service);
        }
    }
}