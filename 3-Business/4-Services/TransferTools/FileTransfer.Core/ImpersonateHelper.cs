using FileTransfer.DomainModel.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileTransfer.Core
{
    public class ImpersonateHelper
    {
         
        public Config Config { get; set; }

        Impersonate Impersonate { get; set; }

        ImpersonateConfig Settings
        {
            get
            {
                return Config.Impersonate;
            }
        }

        public ImpersonateHelper(Config config)
        {
            this.Config = config;
            Impersonate = new Impersonate();
        }

        public void BeginImpersonate()
        {
            try
            {
                if (Settings.Enabled)
                {
                    
                    Boolean activated = Impersonate.ImpersonateValidUser(Settings.Username, Settings.Domain, Settings.Password);
                    if (activated)
                    {
                        
                    }
                    else
                    {
                        
                    }
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                
                //throw ex;
            }
            
        }

        public void EndImpersonate()
        {
            try
            {
                if (Settings.Enabled)
                {
                    
                    Impersonate.UndoImpersonation();                    
                }
            }
            catch (Exception ex)
            {
                
                //throw ex;                
            }
            
        }
    }
}
