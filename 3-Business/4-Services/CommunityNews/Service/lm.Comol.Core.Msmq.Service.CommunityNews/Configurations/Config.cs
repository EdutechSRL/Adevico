using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Msmq.Service.CommunityNews.Configurations
{
    [Serializable]
    public class Config
    {
        public String ServiceName { get; set; }
        public String ServiceDescription { get; set; }
        //service.ServiceName = "COMOL_ErrorService";
        //service.Description = "COMOL_ErrorService Windows Service Host";
        
        public Config()
        {
            ServiceName = "ServiceName";
            ServiceDescription = "ServiceDescription";
        }
    }
}
