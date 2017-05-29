using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using WinService.Configurations;

namespace lm.ErrorsNotification.WinSrvHost
{
    [RunInstaller(true)]
    public partial class ErrorServiceInstaller : Installer
    {
        private ServiceProcessInstaller process;
         private ServiceInstaller service;

    public ErrorServiceInstaller()
    {
        process = new ServiceProcessInstaller();
        process.Account = ServiceAccount.LocalSystem;
        service = new ServiceInstaller();
        //service.ServiceName = "COMOL_ErrorService";
        //service.Description = "COMOL_ErrorService Windows Service Host";

        service.ServiceName = Configuration.Cfg.ServiceName;
        service.Description = Configuration.Cfg.ServiceDescription;

        service.StartType = ServiceStartMode.Automatic;
        Installers.Add(process);
        Installers.Add(service);
        this.Committed += new InstallEventHandler(ErrorServiceInstaller_Committed);
    }

    void ErrorServiceInstaller_Committed(object sender, InstallEventArgs e)
    {
        var controller = new ServiceController(Configuration.Cfg.ServiceName);
        controller.Start();
    }

    }
}