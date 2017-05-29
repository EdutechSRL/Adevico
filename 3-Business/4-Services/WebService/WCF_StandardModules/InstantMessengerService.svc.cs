using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WCF_StandardModules.InstantMessenger;
using WCF_StandardModules.InstantMessenger.Domain;

namespace WCF_StandardModules
{
    /// <summary>
    /// Service per la gestione della chat singola
    /// </summary>
    /// <remarks>
    ///  - Eventualmente ottimizzare con IDictionary IdUtente/GuidChat + IDictionary IdChat/Chat
    ///  - Valutare se restituire l'intera chat (versione attuale) o versioni più ottimizzate
    /// </remarks>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class InstantMessengerService : IInstantMessengerService
    {
        private IMSingletonManager oChatManager;


        InstantMessengerService()
        {
            oChatManager = new IMSingletonManager();        
        }

        public IMMessagesContainerDTO GetChat(Guid ChatId, Guid UserId)
        {
            return oChatManager.GetChat(ChatId, UserId);
        }

        public IMMessagesContainerDTO CreateChat(IMUserDTO StartUser, IMUserDTO TargetUser)
        {
            return oChatManager.StarChat(StartUser, TargetUser);
        }


        //public void SetChat_TEST()
        //{
        //    oChatManager.SetTest();
        //}

        //public IList<Chat1o1.Domain.Ct1o1_MessagesContainer_DTO> GetChat_TEST()
        //{
        //    return oChatManager.GetTest();
        //}


        public IList<IMChatDTO> GetChats(Guid UserId)
        {
           return this.oChatManager.GetChats(UserId);
        }

        public void SendMessage(Guid ChatId, Guid SenderId, string Message)
        {
            this.oChatManager.SendMessage(SenderId, ChatId, Message);
        }

        public Int32 PersonIdByUserId(Guid UserId)
        {
            //TODO: Obtain PersonId By UserId
            return 0;
        }

        public void DiscardChat(Guid SenderId, Guid ChatId)
        {
            Int32 PersonId = PersonIdByUserId(SenderId);
            oChatManager.DiscardChat(PersonId, ChatId);
        }

        void IInstantMessengerService.DiscardAllChats(Guid UserId)
        {
            List<IMChatDTO> Chats = new List<IMChatDTO>();

            Int32 PersonId = PersonIdByUserId(UserId);

            foreach (IMChatDTO Chat in Chats)
            {
                oChatManager.DiscardChat(PersonId, Chat.Id);
            }

        }
    }
}
