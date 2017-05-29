using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lm.Comol.Core.Msmq.Services.Notifications.Service.Config
{
    [Serializable]
    public class ModuleConfig : MailConfig
    {
        public String ModuleCode { get; set; }
    }
}
