using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lm.Comol.Core.Mail;
using lm.Comol.Core.Msmq.Services.Mail.DataContract.Domain;

namespace lm.Comol.Core.Msmq.Services.Mail.Service.Config
{
    [Serializable]
    public class IstanceConfig
    {
        public string UniqueIdentifier {get;set;}
        public string Name {get;set;}
        public string AttachmentUploadPath {get;set;}
        public string AttachmentSentPath {get;set;}
        public string ConnectionString {get;set;}
        public Boolean SaveAttachments {get;set;}
        public Boolean Enabled {get;set;}
        public lm.Comol.Core.MailCommons.Domain.Configurations.SmtpServiceConfig SmtpConfig { get; set; }
    }
}
