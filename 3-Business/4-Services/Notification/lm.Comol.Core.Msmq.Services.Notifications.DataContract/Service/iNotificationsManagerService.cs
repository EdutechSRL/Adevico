using System;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using lm.Comol.Core.Msmq.Services.Notifications.DataContract;

namespace lm.Comol.Core.Msmq.Services.Notifications.Service
{
    [ServiceContract]
    public interface iNotificationsManagerService
    {
        /// <summary>
        /// Send notification action and each modules MUST find ways to notify to users
        /// </summary>
        /// <param name="istanceIdentifier">Istance unique identifier</param>
        /// <param name="action">action to notify</param>
        /// <param name="idSenderUser">optional, id of user who send notification message</param>
        [OperationContract(IsOneWay = true)]
        void NotifyActionToModule(String istanceIdentifier, NotificationAction action, Int32 idSenderUser = 0);

        /// <summary>
        /// Send notification action and tell service which channel MUST use
        /// </summary>
        /// <param name="istanceIdentifier">Istance unique identifier</param>
        /// <param name="channel">channel to use for notification</param>
        /// <param name="mode"></param>
        /// <param name="action">action to notify</param>
        /// <param name="idSenderUser">optional, id of user who send notification message</param>
        [OperationContract(IsOneWay = true)]
        void NotifyAction(String istanceIdentifier, lm.Comol.Core.Notification.Domain.NotificationChannel channel, lm.Comol.Core.Notification.Domain.NotificationMode mode, NotificationAction action, Int32 idSenderUser = 0);

    }
}