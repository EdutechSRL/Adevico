using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace FileTransfer.Core
{
    [ServiceContract]
    public interface IUnzipMQService
    {
        [OperationContract(IsOneWay = true)]
        void FileUnzipNotification(String Platform, Guid FileId);
    }
}
