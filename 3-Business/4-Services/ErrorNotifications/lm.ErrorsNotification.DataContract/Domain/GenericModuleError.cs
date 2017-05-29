﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.ErrorsNotification.DataContract.Domain
{
    [DataContract]
    public class GenericModuleError
    {
        [DataMember]
        public System.Guid UniqueID { get; set; }
        
        [DataMember]
        public string ModuleCode { get; set; }

        [DataMember]
        public DateTime SentDate { get; set; }

        [DataMember]
        public DateTime Day { get; set; }
        
        [DataMember]
        public String Message { get; set; }

        [DataMember]
        public String InnerExceptionMessage { get; set; }

        [DataMember]
        public String ComolUniqueID { get; set; }

        [DataMember]
        public PersistTo Persist { get; set; }

        [DataMember]
        public ErrorType Type { get; set; }

        public GenericModuleError() {
            this.Type = ErrorType.GenericModuleError;
        }

        public GenericModuleError(String comolUniqueID, PersistTo persist)
        {
            this.UniqueID = Guid.NewGuid();
            this.SentDate = DateTime.Now;
            this.Day = this.SentDate.Date;
            this.ComolUniqueID = comolUniqueID;
            this.Persist = persist;
            this.Type = ErrorType.GenericModuleError;
        }

        public GenericModuleError(String moduleCode, String message, String innerExceptionMessage, String comolUniqueID, PersistTo persist)
        {
            this.UniqueID = Guid.NewGuid();
            this.SentDate = DateTime.Now;
            this.Day = this.SentDate.Date;
            this.Message = message;
            this.ModuleCode = moduleCode;
            this.InnerExceptionMessage = innerExceptionMessage;
            this.ComolUniqueID = comolUniqueID;
            this.Persist = persist;
            this.Type = ErrorType.GenericModuleError;
        }
    }
}