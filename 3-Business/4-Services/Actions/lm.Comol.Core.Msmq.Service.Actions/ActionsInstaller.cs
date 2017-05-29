using lm.Comol.Core.Msmq.Service.Actions.Configurations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace lm.Comol.Core.Msmq.Service.Actions
{
    [RunInstaller(true)]

    public partial class ActionsInstaller : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public ActionsInstaller()
        {
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service = new ServiceInstaller();
            //service.ServiceName = "COMOL_ActionsSvcHost";
            //service.Description = "COMOL Actions Windows Service Host";

            service.ServiceName = Configuration.Cfg.ServiceName;
            service.Description = Configuration.Cfg.ServiceDescription;

            service.StartType = ServiceStartMode.Automatic;
            Installers.Add(process);
            Installers.Add(service);

        }
    }
}
