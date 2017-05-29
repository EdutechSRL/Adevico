using lm.Comol.Core.Msmq.Service.CommunityNews.Configurations;
using System.ServiceProcess;


namespace lm.Comol.Core.Msmq.Service.CommunityNews
{
    partial class CommunityNewsWinServiceInstaller
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


        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        ///// <summary>
        ///// Required designer variable.
        ///// </summary>
        //private System.ComponentModel.IContainer components = null;

        ///// <summary> 
        ///// Clean up any resources being used.
        ///// </summary>
        ///// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //#region Component Designer generated code

        ///// <summary>
        ///// Required method for Designer support - do not modify
        ///// the contents of this method with the code editor.
        ///// </summary>
        //private void InitializeComponent()
        //{
        //    this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
        //    this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
        //    // 
        //    // serviceProcessInstaller1
        //    // 
        //    this.serviceProcessInstaller1.Password = null;
        //    this.serviceProcessInstaller1.Username = null;
        //    // 
        //    // serviceInstaller1
        //    // 
        //    service.ServiceName = Configuration.Cfg.ServiceName;
        //    service.Description = Configuration.Cfg.ServiceDescription;
        //    this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
        //    this.serviceInstaller1.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller1_AfterInstall);
        //    // 
        //    // NotificationWinServiceInstaller
        //    // 
        //    this.Installers.AddRange(new System.Configuration.Install.Installer[] {
        //    this.serviceProcessInstaller1,
        //    this.serviceInstaller1});
        //    this.Committed += new System.Configuration.Install.InstallEventHandler(this.NotificationWinServiceInstaller_Committed);

        //}

        //void NotificationWinServiceInstaller_Committed(object sender, System.Configuration.Install.InstallEventArgs e)
        //{
        //    var controller = new ServiceController(Configuration.Cfg.ServiceName);
        //    controller.Start();

        //}

        //#endregion

        //private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
        //private System.ServiceProcess.ServiceInstaller serviceInstaller1;
    }
}