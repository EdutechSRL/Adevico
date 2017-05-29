using FileTransfer.DomainModel.Configuration;
using FileTransfer.WCFService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace FileTransfer.WCFService.Contracts
{
    [ServiceContract()]
    public interface IFileTransferService
    {
        [OperationContract(IsOneWay = true)]
        void Upload(FileTransferRequest request);

        [OperationContract(IsOneWay = true)]
        void Dir(String[] files);
    }
}
