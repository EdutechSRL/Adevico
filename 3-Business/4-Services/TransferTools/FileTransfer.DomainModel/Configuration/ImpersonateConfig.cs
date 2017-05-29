using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileTransfer.DomainModel.Configuration
{
    [Serializable]
    public class ImpersonateConfig
    {
        public ImpersonateConfig()
        {
            Enabled = false;
            Username = "Username";
            Password = "Password";
            Domain = "Domain";
        }

        public String Domain { get; set; }
        public Boolean Enabled { get; set; }
        public String Password { get; set; }
        public String Username { get; set; }
    }
}
