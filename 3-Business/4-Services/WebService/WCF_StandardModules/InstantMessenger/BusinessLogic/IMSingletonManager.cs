using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using lm.Comol.Core.Cache;
using WCF_StandardModules.InstantMessenger.Domain;

namespace WCF_StandardModules.InstantMessenger
{
    public class IMSingletonManager
    {
        public IMSingletonManager()
        {
            this._Chats = new List<IMMessagesContainerDTO>();
        }

        /// <summary>
        /// Inizializza una chat, se non è già presente.
        /// </summary>
        /// <param name="InitUser">L'utente che cerca di aprire la chat.</param>
        /// <param name="ReceiveUser">L'utente "ricevente"</param>
        /// <returns>Nel primo caso restituisce la nuova chat, nel secondo quella corretta.</returns>
        public IMMessagesContainerDTO StarChat(IMUserDTO InitUser, IMUserDTO ReceiveUser)
        {
            IMMessagesContainerDTO TempChat = (from ct in Chats
                                                           where ((ct.UserA.Id == InitUser.Id) && (ct.UserB.Id == ReceiveUser.Id)
                                                           || (ct.UserA.Id == ReceiveUser.Id) && (ct.UserB.Id == InitUser.Id))
                                                           select ct).FirstOrDefault();
           if (TempChat != null)
           {
               //Restituisco la chat corretta
               return TempChat;
           }
            else
           {
               //Creo la nuova chat
               TempChat = new Domain.IMMessagesContainerDTO();
               TempChat.Id = Guid.NewGuid();

               TempChat.UserA = InitUser;
               TempChat.UserB = ReceiveUser;

               TempChat.UserA.LastAccess = DateTime.Now;
               TempChat.UserB.LastAccess = DateTime.Now;

               TempChat.UserA.IsActive = true;
               TempChat.UserB.IsActive = false;

               TempChat.UserA.IsEnter = true;
               TempChat.UserB.IsEnter = false;
               //TempChat.Person1.IsChatvisible = true;
               //TempChat.Person2.IsChatvisible = true;

               TempChat.IsStarted = false;
               
               //Original: TempChat.UserDiscarded = ReceiveUser.Id;
               TempChat.UserDiscarded = ReceiveUser.PersonId;

               TempChat.Messages = new List<IMMessageDTO>();

               Chats.Add(TempChat);

               return TempChat;
               //    this.Messages = new List<Ct1o1_Message_DTO>();
           }
        }

        public bool SendMessage(Guid SenderId, Guid ChatId, String MessageText)
        {
            IMMessagesContainerDTO Chat = GetChat(ChatId, SenderId);
           
            //Se uno degli utenti è di quella chat...
            if (Chat.UserA.Id == SenderId || Chat.UserB.Id == SenderId)
            {
                DateTime Date = DateTime.Now;
                IMMessageDTO Message = new Domain.IMMessageDTO();
                Message.Date = Date;
                Message.MessageText = MessageText;
                Message.SenderId = SenderId;

                Chat.Messages.Add(Message);

                if (Chat.UserA.Id == SenderId)
                {
                    Chat.UserA.LastAccess = Date;
                }
                else {
                    Chat.UserB.LastAccess = Date;
                }
            }
            else {
                throw new ArgumentException("Wrong chat or user");
            }

            return true;
        }
        //private DateTime GetLastAccess(Int32 PersonId)
        //{
        //    DateTime DT;
        //    if( Person1.Id == PersonId)
        //    {
        //        DT = Person1.LastAccess;
        //        Person1.LastAccess = DateTime.Now;
        //    }
        //    else if (Person2.Id == PersonId)
        //    {
        //        DT = Person2.LastAccess;
        //        Person2.LastAccess = DateTime.Now;
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Persons ID not in this chat");
        //    }

        //    return DT;
        //}

        //private void UpdateUser(Int32 UserId)
        //{
        //    if (_UserDiscarded == UserId)
        //    {
        //        _UserDiscarded = 0;
        //        IsActive = true;
        //    }
        //}

        /// <summary>
        /// Restituisce una Chat
        /// </summary>
        /// <param name="ChatId"></param>
        /// <returns></returns>
        /// <remarks>Restituire NULL se la chat non c'è più.</remarks>
        public Domain.IMMessagesContainerDTO GetChat(Guid ChatId, Guid UserId)
        {
            IMMessagesContainerDTO Chat = (from ct in Chats where ct.Id == ChatId select ct).FirstOrDefault<IMMessagesContainerDTO>();

            if (Chat == null)
            {
                Chat = new IMMessagesContainerDTO();
                Chat.Id = ChatId;
            }
            else {
                if (Chat.UserA.Id == UserId)
                {
                    Chat.UserA.IsEnter = true;
                } else if (Chat.UserB.Id == UserId)
                { 
                    Chat.UserB.IsEnter = true;
                }
            }

            return Chat;
        }
        public IList<IMMessagesContainerDTO> GetMessagesContainers(Guid UserId)
        {
            return (from ct in Chats 
                    where (ct.UserA.Id == UserId) || (ct.UserA.Id == UserId) 
                    select ct).ToList<IMMessagesContainerDTO>();
        }

        public IList<IMChatDTO> GetChats(Guid UserId)
        {
            return (from ct in Chats
                    where (ct.UserA.Id == UserId) || (ct.UserB.Id == UserId)
                    select new IMChatDTO { 
                        Id = ct.Id, IsActive=ct.IsActive, 
                        IsStarted=ct.IsStarted, 
                        UserA = ct.UserA, 
                        UserB=ct.UserB, 
                        UserDiscarded = ct.UserDiscarded,
                        MessagesCount = ct.Messages.Count()})
                    .ToList<IMChatDTO>();
        }

        public void DiscardChat(Int32 UserDiscardId, Guid ChatId)
        {
            IMMessagesContainerDTO Chat = (from ct in Chats where ct.Id == ChatId select ct).First();

            if (Chat != null)
            { 
                if (Chat.IsActive)
                {
                    Chat.IsActive = false;
                    Chat.UserDiscarded = UserDiscardId;
                } else if (Chat.UserDiscarded != UserDiscardId)
                {
                    Chats.Remove(Chat);
                    //this.DeleteChat(ChatId);        
                }
            }
        }

        /// <summary>
        /// TUTTE le chat attualmente in uso...
        /// </summary>
        private IList<IMMessagesContainerDTO> Chats
        {
            get
            {
                return this._Chats;
            }

            set
            {
                this._Chats = value;
            }
        }
        private IList<IMMessagesContainerDTO> _Chats;

        //public void SetTest()
        //{
        //    String CacheTestKey = Guid.NewGuid().ToString();

            
            
        //    Domain.Ct1o1_User_DTO Ut1 = new Domain.Ct1o1_User_DTO();
        //    Ut1.Id = 1;
        //    Ut1.DisplayName = "Ut1";
        //    Ut1.IsActive = true;
        //    Ut1.IsChatvisible = true;
        //    Ut1.LastAccess = DateTime.Now;

        //    Domain.Ct1o1_User_DTO Ut2 = new Domain.Ct1o1_User_DTO();
        //    Ut1.Id = 2;
        //    Ut1.DisplayName = "Ut2";
        //    Ut1.IsActive = true;
        //    Ut1.IsChatvisible = false;
        //    Ut1.LastAccess = DateTime.Now;

        //    Domain.Ct1o1_User_DTO Ut3 = new Domain.Ct1o1_User_DTO();
        //    Ut1.Id = 3;
        //    Ut1.DisplayName = "Ut3";
        //    Ut1.IsActive = false;
        //    Ut1.IsChatvisible = false;
        //    Ut1.LastAccess = DateTime.Now;

        //    Domain.Ct1o1_Message_DTO Msg1 = new Domain.Ct1o1_Message_DTO();
        //    Msg1.SenderId = 1;
        //    Msg1.MessageText = "test test test";

        //    Domain.Ct1o1_Message_DTO Msg2 = new Domain.Ct1o1_Message_DTO();
        //    Msg2.SenderId = 1;
        //    Msg2.MessageText = "2 test 2 test 2 test 2";

        //    Domain.Ct1o1_Message_DTO Msg3 = new Domain.Ct1o1_Message_DTO();
        //    Msg3.SenderId = 2;
        //    Msg3.MessageText = "2 test 2 test 2 test 2";



        //    Domain.Ct1o1_MessagesContainer_DTO Cht1 = new Domain.Ct1o1_MessagesContainer_DTO();
        //    Cht1.Id = Guid.NewGuid();
        //    Cht1.IsActive = true;
        //    Cht1.IsStarted = true;
        //    Cht1.Messages = new List<Domain.Ct1o1_Message_DTO>();
        //    Cht1.Messages.Add(Msg1);
        //    Cht1.Messages.Add(Msg2);
        //    Cht1.Messages.Add(Msg3);

        //    Cht1.Person1 = Ut1;
        //    Cht1.Person2 = Ut2;

        //    Cht1.UserDiscarded = 0;

        //    Domain.Ct1o1_MessagesContainer_DTO Cht2 = new Domain.Ct1o1_MessagesContainer_DTO();
        //    Cht2.Id = Guid.NewGuid();
        //    Cht2.IsActive = true;
        //    Cht2.IsStarted = true;
        //    Cht1.Messages = new List<Domain.Ct1o1_Message_DTO>();

        //    Cht2.Person1 = Ut2;
        //    Cht2.Person2 = Ut3;

        //    Cht2.UserDiscarded = 0;

        //    this._Chats.Add(Cht1);
        //    this._Chats.Add(Cht2);

        //}


        //public IList<Domain.Ct1o1_MessagesContainer_DTO> GetTest()
        //{
        //    return this._Chats;
        //}


    }
}