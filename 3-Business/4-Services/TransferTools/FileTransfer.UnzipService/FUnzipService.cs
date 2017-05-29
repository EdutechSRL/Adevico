using FileTransfer.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace FileTransfer.UnzipService
{
    public partial class FUnzipService : ServiceBase
    {
        

        StandardUnzipServiceHost ssh = new StandardUnzipServiceHost();

        public FUnzipService()
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
