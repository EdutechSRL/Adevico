using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lm.Comol.Core.Msmq.Services.Notifications.Service.Config
{
    [Serializable]
    public class MailConfig
    {
        public string Address {get;set;}
        public string Binding {get;set;}
        public Boolean Enabled {get;set;}
        public Boolean TryToLoadFromDB { get; set; }

    }
}