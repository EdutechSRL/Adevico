using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using lm.ErrorsNotification.DataContract.Domain;

namespace lm.ErrorsNotification.DataContract.Service
{
    [ServiceContract]
    public interface iErrorsNotificationService
    {
        [OperationContract(IsOneWay=true)]
        void sendCommunityModuleError(CommunityModuleError error);
        
        [OperationContract(IsOneWay = true)]
        void sendDBerror(DBerror error);
        
        [OperationContract(IsOneWay = true)]
        void sendGenericError(GenericError error);

        [OperationContract(IsOneWay = true)]
        void sendGenericModuleError(GenericModuleError error);

        [OperationContract(IsOneWay = true)]
        void sendGenericWebError(GenericWebError error);

        [OperationContract(IsOneWay = true)]
        void sendFileError(FileError error);
        
    }
}