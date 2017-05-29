using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using lm.ActionPersistence;
using System.ServiceModel;
using System.Threading;
namespace lm.Comol.Core.Msmq.Service.Actions
{
    public partial class Service : ServiceBase
    {
        public Service()
        {
            InitializeComponent();
        }

        //private int Delay;

        ServiceHost host;

        protected override void OnStart(string[] args)
        {

            //Delay = int.Parse(ConfigurationManager.AppSettings["DelaySeconds"]);
            //Thread.Sleep(Delay * 1000);

            //string queueName = ConfigurationManager.AppSettings["ActionsQueueName"];
            //string poisonQueueName = ConfigurationManager.AppSettings["PoisonActionsQueueName"];

            //if (!MessageQueue.Exists(queueName))
            //    MessageQueue.Create(queueName, true);

            //if (!MessageQueue.Exists(poisonQueueName))
            //    MessageQueue.Create(poisonQueueName, true);

            Type serviceType = typeof(ActionService);
            host = new ServiceHost(serviceType);

            //using (host = new ServiceHost(serviceType))
            // {
            //     host.Opening += new EventHandler(host_Opening);
            //     host.Opened += new EventHandler(host_Opened);
            //     host.Faulted += new EventHandler(host_Faulted);
            //     host.Closing += new EventHandler(host_Closing);
            //     host.Closed += new EventHandler(host_Closed);
            //     host.UnknownMessageReceived += new EventHandler<UnknownMessageReceivedEventArgs>(host_UnknownMessageReceived);

            host.Open();
            //}

        }

        protected override void OnStop()
        {
            host.Close();
        }

        void host_UnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
        {
            EventLogger.WriteEntry(e.Message.ToString(), EventLogEntryType.Error);
        }

        void host_Faulted(object sender, EventArgs e)
        {
            EventLogger.WriteEntry("Faulted", EventLogEntryType.Error);
        }

        void host_Closed(object sender, EventArgs e)
        {
            EventLogger.WriteEntry("ActionsWinSvcHost stopped", EventLogEntryType.Information);
        }

        void host_Closing(object sender, EventArgs e)
        {
            EventLogger.WriteEntry("ActionsWinSvcHost stopping...", EventLogEntryType.Information);
        }

        void host_Opened(object sender, EventArgs e)
        {
            EventLogger.WriteEntry("ActionsWinSvcHost started", EventLogEntryType.Information);
        }

        void host_Opening(object sender, EventArgs e)
        {
            EventLogger.WriteEntry("ActionsWinSvcHost starting...", EventLogEntryType.Information);
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
