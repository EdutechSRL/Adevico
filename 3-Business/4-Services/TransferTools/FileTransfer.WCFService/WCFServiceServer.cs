using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.ServiceModel;
using System.Text;

namespace FileTransfer.WCFService
{
    public partial class WCFServiceServer : ServiceBase
    {
        
        //FileServiceHost ssh = new FileServiceHost();
        StandardWCFServiceHost ssh = new StandardWCFServiceHost();

        public WCFServiceServer()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {            
            ssh.StartService();
        }

        protected override void OnStop()
        {
            ssh.StopService();
        }

        public void ServiceStart(string[] args)
        {
            OnStart(args);
        }

        public void ServiceStop()
        {
            OnStop();
        }
    }
}
