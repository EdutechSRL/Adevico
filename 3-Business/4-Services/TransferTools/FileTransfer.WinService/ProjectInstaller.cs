using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace FileTransfer.WinService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            
        }

        protected override void OnAfterInstall(IDictionary savedState) {
            base.OnAfterInstall(savedState);

            ServiceController sc = new ServiceController(serviceInstaller.ServiceName);
            sc.Start();
        }
    }
}
