using lm.Comol.Core.Msmq.Services.Mail.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace lm.Comol.Core.Msmq.Services.Mail.WinSrvHost
{
    public partial class WINmailService : ServiceBase
    {
        ServiceHost host;
        public WINmailService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //string queueName = ConfigurationManager.AppSettings["NotificationQueueName"];
            //string poisonQueueName = ConfigurationManager.AppSettings["PoisonNotificationQueueName"];

            //if (!MessageQueue.Exists(queueName))
            //    MessageQueue.Create(queueName, true);

            //if (!MessageQueue.Exists(poisonQueueName))
            //    MessageQueue.Create(poisonQueueName, true);

            host = new ServiceHost(typeof(ServiceMailSender));
            host.Open();
        }

        protected override void OnStop()
        {
            host.Close();
        }
        void host_Faulted(object sender, EventArgs e)
        {
            SRVeventLog.WriteEntry("Faulted", EventLogEntryType.Error);
        }

        void host_UnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
        {
            SRVeventLog.WriteEntry(e.Message.ToString(), EventLogEntryType.Error);
        }

        void host_Closed(object sender, EventArgs e)
        {
            SRVeventLog.WriteEntry("COMOL_ErrorService stopped", EventLogEntryType.Information);
        }

        void host_Closing(object sender, EventArgs e)
        {
            SRVeventLog.WriteEntry("COMOL_ErrorService stopping...", EventLogEntryType.Information);
        }

        void host_Opened(object sender, EventArgs e)
        {
            SRVeventLog.WriteEntry("COMOL_ErrorService started", EventLogEntryType.Information);
        }

        void host_Opening(object sender, EventArgs e)
        {
            SRVeventLog.WriteEntry("COMOL_ErrorService starting...", EventLogEntryType.Information);
        }

        private void SRVeventLog_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }
    }
}


//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Diagnostics;
//using System.Linq;
//using System.ServiceProcess;
//using System.Text;
//using System.Configuration;
//using System.Messaging;
//using System.ServiceModel;
//using lm.ErrorsNotification.DataContract;
//using lm.ErrorsNotification.Service ;

//namespace lm.ErrorsNotification.WinSrvHost
//{
//    partial class WINerrorsNotificationsService : ServiceBase
//    {
//        void host_Faulted(object sender, EventArgs e)
//        {
//            SRVeventLog.WriteEntry("Faulted", EventLogEntryType.Error);
//        }

//        void host_UnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
//        {
//            SRVeventLog.WriteEntry(e.Message.ToString(), EventLogEntryType.Error);
//        }

//        void host_Closed(object sender, EventArgs e)
//        {
//            SRVeventLog.WriteEntry("COMOL_ErrorService stopped", EventLogEntryType.Information);
//        }

//        void host_Closing(object sender, EventArgs e)
//        {
//            SRVeventLog.WriteEntry("COMOL_ErrorService stopping...", EventLogEntryType.Information);
//        }

//        void host_Opened(object sender, EventArgs e)
//        {
//            SRVeventLog.WriteEntry("COMOL_ErrorService started", EventLogEntryType.Information);
//        }

//        void host_Opening(object sender, EventArgs e)
//        {
//            SRVeventLog.WriteEntry("COMOL_ErrorService starting...", EventLogEntryType.Information);
//        }

//        private void SRVeventLog_EntryWritten(object sender, EntryWrittenEventArgs e)
//        {

//        }
//    }
//}
