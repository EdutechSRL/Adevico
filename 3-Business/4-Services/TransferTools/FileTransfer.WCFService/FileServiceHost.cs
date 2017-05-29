using FileTransfer.DomainModel.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace FileTransfer.WCFService
{
    public class FileServiceHost
    {
        
        protected ServiceHost MyServiceHost = null;
        private Config Cfg = ConfigurationLoader.Configuration;

        public void StartService()
        {
            try
            {

                MyServiceHost = new ServiceHost(typeof(WCFFileTransfer));

                MyServiceHost.Opening += MyServiceHost_Opening;
                MyServiceHost.Opened += MyServiceHost_Opened;
                MyServiceHost.Faulted += MyServiceHost_Faulted;
                MyServiceHost.Closing += MyServiceHost_Closing;
                MyServiceHost.Closed += MyServiceHost_Closed;

                MyServiceHost.Open();

            }
            catch (Exception ex)
            {
                MyServiceHost = null;
                
            }
        }

        void MyServiceHost_Closing(object sender, EventArgs e)
        {
            
        }

        void MyServiceHost_Closed(object sender, EventArgs e)
        {
            
        }

        void MyServiceHost_Faulted(object sender, EventArgs e)
        {
            
        }

        void MyServiceHost_Opened(object sender, EventArgs e)
        {
            
        }

        void MyServiceHost_Opening(object sender, EventArgs e)
        {
            
        }

        public void StopService()
        {
            try
            {
                if (MyServiceHost != null)
                {
                    MyServiceHost.Close();
                }
            }
            catch (Exception ex)
            {
                MyServiceHost = null;
                
            }
        }
    }
}
