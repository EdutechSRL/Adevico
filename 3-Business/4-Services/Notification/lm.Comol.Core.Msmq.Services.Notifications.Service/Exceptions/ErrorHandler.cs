using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Transactions;
using System.Configuration;
using lm.Comol.Core.Notification.Domain;
using lm.Comol.Core.Notification.DataContract;

namespace lm.Comol.Core.Msmq.Services.Notifications.Service.Exceptions
{
    public static class ErrorHandler
    {
        public static void addActionToPoisonQueue(NotificationAction action, NotificationException error, String poisonQueue = "")
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
                        poisonMessageQueue.Send(CreateErrorMessage(action, error), MessageQueueTransactionType.Single);
                        txScope.Complete();
                    }
                }
            }
        }
        public static void addActionToPoisonQueue(NotificationAction action, ExceptionType errorType,Exception error,  String poisonQueue = "")
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
                        poisonMessageQueue.Send(CreateErrorMessage(action,errorType,error), MessageQueueTransactionType.Single);
                        txScope.Complete();
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action">NotificationAction </param>
        /// <param name="poisonQueue">queue name</param>
        public static void addActionToPoisonQueue(NotificationAction action, ExceptionType errorType, String poisonQueue = "")
        {
            if (String.IsNullOrEmpty(poisonQueue))
                poisonQueue = System.Configuration.ConfigurationManager.AppSettings["PoisonErrorsQueueName"];
            if (null != action && !String.IsNullOrEmpty(poisonQueue))
            {
                MessageQueue poisonMessageQueue = new MessageQueue(poisonQueue);
                if (poisonMessageQueue != null)
                {
                    using (TransactionScope txScope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        poisonMessageQueue.Send(CreateErrorMessage(action,errorType), MessageQueueTransactionType.Single);
                        txScope.Complete();
                    }
                }
            }
        }
        private static ErrorMessage CreateErrorMessage(NotificationAction action, ExceptionType errorType,Exception error=null)
        {
            ErrorMessage obj = new ErrorMessage() { Action = action};
            if (error != null)
            {
                obj.Error.HelpLink = error.HelpLink;
                obj.Error.StackTrace = error.StackTrace;
                obj.Error.Source = error.Source;
                obj.Error.Message = error.Message;
                obj.Error.InnerException = (error.InnerException == null) ? "" : error.InnerException.Message;
            }
            obj.Error.Type = errorType.ToString();
            return obj;
        }
        private static ErrorMessage CreateErrorMessage(NotificationAction action, NotificationException error)
        {
            ErrorMessage obj = CreateErrorMessage(action, error.Type);
            obj.Error.StackTrace = error.StackTrace;
            obj.Error.Source = error.Source;
            obj.Error.Message = error.Message;
            obj.Error.InnerException = (error.InnerException == null) ? "" : error.InnerException.Message;
            return obj;
        }
    }
}