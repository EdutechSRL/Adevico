using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace lm.ErrorsNotification.DataContract.Domain
{
    [DataContract]
    public class FileError
    {
        [DataMember]
        public System.Guid UniqueID { get; set; }
        
        [DataMember]
        public DateTime SentDate { get; set; }

        [DataMember]
        public DateTime Day { get; set; }
        
        [DataMember]
        public int CommunityID { get; set; }

        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public long CommunityFileID { get; set; }

        [DataMember]
        public int ThesisFileID { get; set; }

        [DataMember]
        public long NoticeboardFileID { get; set; }

        [DataMember]
        public System.Guid BaseFileID { get; set; }

        [DataMember]
        public String Message { get; set; }

        [DataMember]
        public String ComolUniqueID { get; set; }

        [DataMember]
        public PersistTo Persist { get; set; }

        [DataMember]
        public ErrorType Type { get; set; }

        public FileError() {
            this.Type = ErrorType.FileError;
        }

        public FileError(String message, String comolUniqueID, PersistTo persist)
        {
            this.UniqueID = Guid.NewGuid();
            this.SentDate = DateTime.Now;
            this.Day = this.SentDate.Date;
            this.Message = message;
            this.ComolUniqueID = comolUniqueID;
            this.Persist = persist;
            this.Type = ErrorType.FileError;
            this.CommunityFileID = 0;
            this.BaseFileID = Guid.Empty;
            this.NoticeboardFileID = 0;
            this.ThesisFileID = 0;
        }
        
        public FileError(long communityFileID,String message, String comolUniqueID, PersistTo persist)
        {
            this.UniqueID = Guid.NewGuid();
            this.SentDate = DateTime.Now;
            this.Day = this.SentDate.Date;
            this.Message = message;
            this.ComolUniqueID = comolUniqueID;
            this.Persist = persist;
            this.Type = ErrorType.FileError;
            this.CommunityFileID = communityFileID;
            this.BaseFileID = Guid.Empty;
            this.NoticeboardFileID = 0;
            this.ThesisFileID = 0;
        }

        public FileError(int thesisFileID,String message, String comolUniqueID, PersistTo persist)
        {
            this.UniqueID = Guid.NewGuid();
            this.SentDate = DateTime.Now;
            this.Day = this.SentDate.Date;
            this.Message = message;
            this.ComolUniqueID = comolUniqueID;
            this.Persist = persist;
            this.Type = ErrorType.FileError;
            this.CommunityFileID = 0;
            this.BaseFileID = Guid.Empty;
            this.NoticeboardFileID = 0;
            this.ThesisFileID = thesisFileID;
        }

        public FileError(System.Guid baseFileID, String message, String comolUniqueID, PersistTo persist)
        {
            this.UniqueID = Guid.NewGuid();
            this.SentDate = DateTime.Now;
            this.Day = this.SentDate.Date;
            this.Message = message;
            this.ComolUniqueID = comolUniqueID;
            this.Persist = persist;
            this.Type = ErrorType.FileError;
            this.CommunityFileID = 0;
            this.BaseFileID = baseFileID;
            this.NoticeboardFileID = 0;
            this.ThesisFileID = 0;
        }
    }
}