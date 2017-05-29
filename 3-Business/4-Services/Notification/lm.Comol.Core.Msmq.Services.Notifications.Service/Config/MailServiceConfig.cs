using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lm.Comol.Core.Msmq.Services.Notifications.Service.Config
{
    [Serializable]
    public class MailServiceConfig
    {
        public Boolean TryToLoadFromDB { get; set; }
        public MailConfig Default { get; set; }
        public List<ModuleConfig> Modules { get; set; }
    }
}
