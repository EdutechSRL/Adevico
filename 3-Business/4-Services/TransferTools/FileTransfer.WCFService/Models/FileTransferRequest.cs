using FileTransfer.DomainModel.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace FileTransfer.WCFService.Models
{
    [MessageContract()]
    public class FileTransferRequest
    {
        [MessageHeader(MustUnderstand = true)]
        public string FileName;

        [MessageHeader(MustUnderstand = true)]
        public string Platform;

        [MessageBodyMember(Order = 1)]
        public System.IO.Stream Data;

        
        //public Platform Platform;
    }
}
