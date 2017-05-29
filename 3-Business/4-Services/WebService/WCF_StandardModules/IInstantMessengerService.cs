using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WCF_StandardModules.InstantMessenger.Domain;

namespace WCF_StandardModules
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IInstantMessengerService" in both code and config file together.
    [ServiceContract]
    public interface IInstantMessengerService
    {
        //TODO: Check if Guid could be replaced by String into WebService Methods
        [OperationContract]
        IMMessagesContainerDTO GetChat(Guid ChatId, Guid UserId);

        [OperationContract]
        IList<IMChatDTO> GetChats(Guid UserId);

        [OperationContract]
        IMMessagesContainerDTO CreateChat(IMUserDTO StartUser, IMUserDTO TargetUser);

        [OperationContract]
        void SendMessage(Guid ChatId, Guid SenderId, String Message);

        [OperationContract]
        void DiscardChat(Guid SenderId, Guid ChatId);
        //[OperationContract]
        //void SetChat_TEST();
        //[OperationContract]
        //IList<Chat1o1.Domain.Ct1o1_MessagesContainer_DTO> GetChat_TEST();

        [OperationContract]
        void DiscardAllChats(Guid UserId);
    }
}
