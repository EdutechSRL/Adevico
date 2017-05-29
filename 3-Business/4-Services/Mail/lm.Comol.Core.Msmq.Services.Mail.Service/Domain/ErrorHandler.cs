using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Transactions;
using System.Configuration;
using lm.Comol.Core.Msmq.Services.Mail.DataContract.Domain.Messages;

namespace lm.Comol.Core.Msmq.Services.Mail.Service
{
    public static class ErrorHandler
    {

        public static void addMessageToPoisonQueue(MailException error, lm.Comol.Core.MailCommons.Domain.Messages.Message message, String poisonQueue = "")
        {
            if (String.IsNullOrEmpty(poisonQueue))
                poisonQueue = System.Configuration.ConfigurationManager.AppSettings["PoisonErrorsQueueName"];
            if (null != error && !String.IsNullOrEmpty(poisonQueue))
            {
                MessageQueue poisonMessageQueue = new MessageQueue(poisonQueue);
                if (poisonMessageQueue != null)
                {
                    using (TransactionScope txScope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        ErrorMessage obj = new ErrorMessage() { Message = message };
                        obj.Error.HelpLink = error.HelpLink;
                        obj.Error.StackTrace = error.StackTrace;
                        obj.Error.Source = error.Source;
                        obj.Error.Message = error.Message;
                        obj.Error.InnerException = (error.InnerException == null) ? "" : error.InnerException.Message;
                        obj.Error.Type = error.Type.ToString();
                        poisonMessageQueue.Send(obj, MessageQueueTransactionType.Single);
                        txScope.Complete();
                    }
                }
            }
        }
        public static void addMessageToPoisonQueue(Exception error,lm.Comol.Core.MailCommons.Domain.Messages.Message message, String poisonQueue="")
        {
            if (String.IsNullOrEmpty(poisonQueue))
                poisonQueue = System.Configuration.ConfigurationManager.AppSettings["PoisonErrorsQueueName"];
            if (null != error && !String.IsNullOrEmpty(poisonQueue))
            {
                MessageQueue poisonMessageQueue = new MessageQueue(poisonQueue);
                if (poisonMessageQueue != null)
                {
                    using (TransactionScope txScope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        ErrorMessage obj = new ErrorMessage() { Message = message };
                        obj.Error.HelpLink = error.HelpLink;
                        obj.Error.StackTrace = error.StackTrace;
                        obj.Error.Source = error.Source;
                        obj.Error.Message = error.Message;
                        obj.Error.InnerException = (error.InnerException == null) ? "" : error.InnerException.Message;

                        poisonMessageQueue.Send(obj, MessageQueueTransactionType.Single);
                        txScope.Complete();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">message </param>
        /// <param name="poisonQueue">queue name</param>
        public static void addMessageToPoisonQueue(lm.Comol.Core.MailCommons.Domain.Messages.Message message, String poisonQueue="")
        {
            if (String.IsNullOrEmpty(poisonQueue))
                poisonQueue = System.Configuration.ConfigurationManager.AppSettings["PoisonErrorsQueueName"];
            if (null != message && !String.IsNullOrEmpty(poisonQueue))
            {
                MessageQueue poisonMessageQueue = new MessageQueue(poisonQueue);
                if (poisonMessageQueue != null)
                {
                    using (TransactionScope txScope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        poisonMessageQueue.Send(message, MessageQueueTransactionType.Single);
                        txScope.Complete();
                    }
                }
            }
        }

        public static void addMessageToPoisonQueue(Exception error, Notification.Domain.dtoModuleNotificationMessage message, String poisonQueue = "")
        {
            if (String.IsNullOrEmpty(poisonQueue))
                poisonQueue = System.Configuration.ConfigurationManager.AppSettings["PoisonErrorsQueueName"];
            if (null != error && !String.IsNullOrEmpty(poisonQueue))
            {
                MessageQueue poisonMessageQueue = new MessageQueue(poisonQueue);
                if (poisonMessageQueue != null)
                {
                    using (TransactionScope txScope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        ErrorNotificationMessage obj = new ErrorNotificationMessage() { Message = message };
                        obj.Error.HelpLink = error.HelpLink;
                        obj.Error.StackTrace = error.StackTrace;
                        obj.Error.Source = error.Source;
                        obj.Error.Message = error.Message;
                        obj.Error.InnerException = (error.InnerException == null) ? "" : error.InnerException.Message;

                        poisonMessageQueue.Send(obj, MessageQueueTransactionType.Single);
                        txScope.Complete();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">message </param>
        /// <param name="poisonQueue">queue name</param>
        public static void addMessageToPoisonQueue(Notification.Domain.dtoModuleNotificationMessage message, String poisonQueue = "")
        {
            if (String.IsNullOrEmpty(poisonQueue))
                poisonQueue = System.Configuration.ConfigurationManager.AppSettings["PoisonErrorsQueueName"];
            if (null != message && !String.IsNullOrEmpty(poisonQueue))
            {
                MessageQueue poisonMessageQueue = new MessageQueue(poisonQueue);
                if (poisonMessageQueue != null)
                {
                    using (TransactionScope txScope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        ErrorNotificationMessage obj = new ErrorNotificationMessage() { Message = message };
 
                        poisonMessageQueue.Send(obj, MessageQueueTransactionType.Single);
                        txScope.Complete();
                    }
                }
            }
        }


        public static void addMessageToPoisonQueue(Exception error, Notification.Domain.GroupMessages group, String poisonQueue = "")
        {
            if (String.IsNullOrEmpty(poisonQueue))
                poisonQueue = System.Configuration.ConfigurationManager.AppSettings["PoisonErrorsQueueName"];
            if (null != error && !String.IsNullOrEmpty(poisonQueue))
            {
                MessageQueue poisonMessageQueue = new MessageQueue(poisonQueue);
                if (poisonMessageQueue != null)
                {
                    using (TransactionScope txScope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        ErrorNotificationGroupMessages obj = new ErrorNotificationGroupMessages() { Group = group };
                        obj.Error.HelpLink = error.HelpLink;
                        obj.Error.StackTrace = error.StackTrace;
                        obj.Error.Source = error.Source;
                        obj.Error.Message = error.Message;
                        obj.Error.InnerException = (error.InnerException == null) ? "" : error.InnerException.Message;

                        poisonMessageQueue.Send(obj, MessageQueueTransactionType.Single);
                        txScope.Complete();
                    }
                }
            }
        }
         /// <summary>
        /// 
        /// </summary>
        /// <param name="message">message </param>
        /// <param name="poisonQueue">queue name</param>
        public static void addMessageToPoisonQueue(Notification.Domain.GroupMessages group, String poisonQueue = "")
        {
            if (String.IsNullOrEmpty(poisonQueue))
                poisonQueue = System.Configuration.ConfigurationManager.AppSettings["PoisonErrorsQueueName"];
            if (null != group && !String.IsNullOrEmpty(poisonQueue))
            {
                MessageQueue poisonMessageQueue = new MessageQueue(poisonQueue);
                if (poisonMessageQueue != null)
                {
                    using (TransactionScope txScope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        ErrorNotificationGroupMessages obj = new ErrorNotificationGroupMessages() { Group = group };
 
                        poisonMessageQueue.Send(obj, MessageQueueTransactionType.Single);
                        txScope.Complete();
                    }
                }
            }
        }
        
    }
}
