using lm.Comol.Core.Msmq.Services.Notifications.Service.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace lm.Comol.Core.Msmq.Services.Notifications.WinSrvHost
{
    public partial class WINnotificationsService : ServiceBase
    {
        ServiceHost host;
        Configuration cfg;
        String serviceName ="TEST";
       
      
        public WINnotificationsService()
        {
            cfg = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            String path = cfg.AppSettings.Settings["ServicePath"].Value;
            ServiceConfig config = lm.Comol.Core.WinService.Configurations.Configuration<ServiceConfig>.Load(path);
            serviceName = config.Service.ServiceName;
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

            host = new ServiceHost(typeof(lm.Comol.Core.Msmq.Services.Notifications.Service.NotificationsService));
            host.Open();
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
            SRVeventLog.WriteEntry(serviceName + " stopped", EventLogEntryType.Information);
        }

        void host_Closing(object sender, EventArgs e)
        {
            SRVeventLog.WriteEntry(serviceName + " stopping...", EventLogEntryType.Information);
        }

        void host_Opened(object sender, EventArgs e)
        {
            SRVeventLog.WriteEntry(serviceName + " started", EventLogEntryType.Information);
        }

        void host_Opening(object sender, EventArgs e)
        {
            SRVeventLog.WriteEntry(serviceName + " starting...", EventLogEntryType.Information);
        }

        private void SRVeventLog_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }
    }
}
