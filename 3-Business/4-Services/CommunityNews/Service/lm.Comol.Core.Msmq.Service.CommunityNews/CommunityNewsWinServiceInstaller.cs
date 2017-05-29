using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using lm.Comol.Core.Msmq.Service.CommunityNews.Configurations;

namespace lm.Comol.Core.Msmq.Service.CommunityNews
{
    [RunInstaller(true)]
    public partial class CommunityNewsWinServiceInstaller : Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public CommunityNewsWinServiceInstaller()
        {
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service = new ServiceInstaller();
            service.ServiceName = Configuration.Cfg.ServiceName;
            service.Description = Configuration.Cfg.ServiceDescription;
            service.StartType = ServiceStartMode.Automatic;
            Installers.Add(process);
            Installers.Add(service);

        }
    }
}
