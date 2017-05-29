using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Transactions;
using System.Configuration;
using lm.ActionDataContract;

namespace lm.ActionPersistence
{
    public class ErrorHandler
    {
        public void addMessageToPoisonQueue(Object obj, Exception error)
        {
            if (null != error)
            {
                MessageQueue poisonMessageQueue = new MessageQueue(ConfigurationManager.AppSettings["PoisonActionsQueueName"]);

                using (TransactionScope txScope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    poisonMessageQueue.Send(obj, DateTime.Now.ToLocalTime().ToShortDateString() + " " + error.Message, MessageQueueTransactionType.Single);
                    txScope.Complete();
                }
               
            }
        }
    }
}
