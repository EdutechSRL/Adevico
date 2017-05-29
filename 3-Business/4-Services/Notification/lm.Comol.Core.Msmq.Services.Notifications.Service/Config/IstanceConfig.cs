using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lm.Comol.Core.Msmq.Services.Notifications.Service.Config
{
    [Serializable]
    public class IstanceConfig
    {
        public string UniqueIdentifier {get;set;}
        public string Name {get;set;}
        public MailServiceConfig MailConfiguration { get; set; }
        public WebConferenceConfig WebConference { get; set; }
        public lm.Comol.Core.Notification.Domain.WebSiteSettings Settings { get; set; }
        
        
        public string ConnectionString {get;set;}
        public Boolean Enabled {get;set;}

        public MailConfig GetMailConfig(String module, Boolean onlyEnabled = true)
        {
            if (!String.IsNullOrEmpty(module))
                module = module.ToLower();
            return (MailConfiguration == null) ? null : ((MailConfiguration.Modules != null && MailConfiguration.Modules.Where(m => ((onlyEnabled && m.Enabled) || !onlyEnabled) && m.ModuleCode.ToLower() == module).Any()) ? MailConfiguration.Modules.Where(m => ((onlyEnabled && m.Enabled) || !onlyEnabled) && m.ModuleCode.ToLower() == module).FirstOrDefault() : MailConfiguration.Default);
        }
    }
}