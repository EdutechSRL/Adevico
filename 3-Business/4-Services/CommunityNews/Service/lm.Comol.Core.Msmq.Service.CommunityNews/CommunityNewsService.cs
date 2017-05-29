using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Messaging;
using System.ServiceModel;
using System.Configuration;
using lm.Notification.Service;

namespace lm.Comol.Core.Msmq.Service.CommunityNews
{
    public partial class CommunityNewsService : ServiceBase
    {
        public CommunityNewsService()
        {
            InitializeComponent();
        }
        ServiceHost host;

        protected override void OnStart(string[] args)
        {
            //string queueName = ConfigurationManager.AppSettings["NotificationQueueName"];
            //string poisonQueueName = ConfigurationManager.AppSettings["PoisonNotificationQueueName"];

            //if (!MessageQueue.Exists(queueName))
            //    MessageQueue.Create(queueName, true);

            //if (!MessageQueue.Exists(poisonQueueName))
            //    MessageQueue.Create(poisonQueueName, true);

            host = new ServiceHost(typeof(NotificationService));
            host.Open();
        }

        protected override void OnStop()
        {
            host.Close();
        }

        void host_UnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
        {
            SRVeventLog.WriteEntry(e.Message.ToString(), EventLogEntryType.Error);
        }

        void host_Faulted(object sender, EventArgs e)
        {
            SRVeventLog.WriteEntry("Faulted", EventLogEntryType.Error);
        }

        void host_Closed(object sender, EventArgs e)
        {
            SRVeventLog.WriteEntry("NotificationsWinSvcHost stopped", EventLogEntryType.Information);
        }

        void host_Closing(object sender, EventArgs e)
        {
            SRVeventLog.WriteEntry("NotificationsWinSvcHost stopping...", EventLogEntryType.Information);
        }

        void host_Opened(object sender, EventArgs e)
        {
            SRVeventLog.WriteEntry("NotificationsWinSvcHost started", EventLogEntryType.Information);
        }

        void host_Opening(object sender, EventArgs e)
        {
            SRVeventLog.WriteEntry("NotificationsWinSvcHost starting...", EventLogEntryType.Information);
        }

        private void SRVeventLog_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }


    }
}
