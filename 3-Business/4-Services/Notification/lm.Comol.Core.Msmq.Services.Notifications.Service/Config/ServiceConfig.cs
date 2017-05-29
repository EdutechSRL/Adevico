using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lm.Comol.Core.Msmq.Services.Notifications.Service.Config
{
    [Serializable]
    public class ServiceConfig
    {
        public Boolean Enabled { get; set; }
        public Boolean WriteLogs { get; set; }
        public lm.Comol.Core.WinService.Configurations.Config Service { get; set; }
        public List<IstanceConfig> Istances { get; set; }
        public ServiceConfig()
        {
            Istances = new List<IstanceConfig>();
        }
        public String GetConfigurationString(String identifier)
        {
            return (Istances == null || !Istances.Where(i => i.UniqueIdentifier == identifier).Any()) ? "" : Istances.Where(i => i.UniqueIdentifier == identifier).FirstOrDefault().ConnectionString;
        }
        public IstanceConfig GetIstanceConfiguration(String identifier)
        {
            return (Istances == null || !Istances.Where(i => i.UniqueIdentifier == identifier).Any()) ? null : Istances.Where(i => i.UniqueIdentifier == identifier).FirstOrDefault();
        }
    }
}
