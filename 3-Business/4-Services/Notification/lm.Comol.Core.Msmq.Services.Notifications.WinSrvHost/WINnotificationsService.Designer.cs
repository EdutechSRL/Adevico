using lm.Comol.Core.Msmq.Services.Notifications.Service.Config;
using System;
using System.Configuration;
using System.Reflection;
namespace lm.Comol.Core.Msmq.Services.Notifications.WinSrvHost
{
    partial class WINnotificationsService
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.SRVeventLog = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.SRVeventLog)).BeginInit();
            // 
            // SRVeventLog
            // 
            Configuration cfg = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            String path = cfg.AppSettings.Settings["ServicePath"].Value;
            ServiceConfig config = lm.Comol.Core.WinService.Configurations.Configuration<ServiceConfig>.Load(path);

            this.SRVeventLog.Log = "Application";
            this.SRVeventLog.Source = config.Service.ServiceName;
            this.SRVeventLog.EntryWritten += new System.Diagnostics.EntryWrittenEventHandler(this.SRVeventLog_EntryWritten);

           // WINerrorsNotificationsService
            this.ServiceName = config.Service.ServiceName;
            ((System.ComponentModel.ISupportInitialize)(this.SRVeventLog)).EndInit();

        }

        private System.Diagnostics.EventLog SRVeventLog;

        #endregion
    }
}
