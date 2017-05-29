using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using FileTransfer.Core;

namespace FileTransfer.WinService
{
    public partial class TransferService : ServiceBase
    {
        

        StandardServiceHost ssh = new StandardServiceHost();

        public TransferService()
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
