﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Mail;

namespace lm.Comol.Core.Mail.Messages
{
    [Serializable]
    public class dtoModuleMessagesContext
    {
        public String MailAddress { get; set; }
        public Int32 IdPerson { get; set; }
        public long IdUserModule { get; set; }
        public Int32 IdCommunity { get; set; }
        public Int32 IdModule { get; set; }
        public String ModuleCode { get; set; }
        public lm.Comol.Core.DomainModel.ModuleObject ModuleObject { get; set; }
    }
}