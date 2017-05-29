namespace lm.ErrorsNotification.WinSrvHost
{
    partial class WINerrorsNotificationsService
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
            this.SRVeventLog = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.SRVeventLog)).BeginInit();
            // 
            // SRVeventLog
            // 
            this.SRVeventLog.Log = "Application";
            this.SRVeventLog.Source = "COMOL_ErrorService";
            this.SRVeventLog.EntryWritten += new System.Diagnostics.EntryWrittenEventHandler(this.SRVeventLog_EntryWritten);
            // 
            // WINerrorsNotificationsService
            // 
            this.ServiceName = "COMOL_ErrorService";
            ((System.ComponentModel.ISupportInitialize)(this.SRVeventLog)).EndInit();

        }

        private System.Diagnostics.EventLog SRVeventLog;

        #endregion
    }
}
