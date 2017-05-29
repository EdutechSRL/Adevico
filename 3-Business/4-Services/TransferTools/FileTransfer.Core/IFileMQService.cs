using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace FileTransfer.Core
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFileMQService" in both code and config file together.
    [ServiceContract]
    public interface IFileMQService
    {
        [OperationContract(IsOneWay=true)]
        void FileTransferNotification(String Platform);

        [OperationContract(IsOneWay = true)]
        void TransferAllFilesDirect(String Platform, List<FileTransferBase> toTransfer);

        [OperationContract(IsOneWay = true)]
        void DirAllFilesDirect(String Platform, String[] Files);
    }
}
