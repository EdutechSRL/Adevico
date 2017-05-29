using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using lm.Notification.DataContract.Domain;

namespace lm.Notification.DataContract.Service
{
    [ServiceContract]


    public interface iNotificationService
    {
        [OperationContract(IsOneWay=true)]
        void NotifyToCommunity(NotificationToCommunity Notification);
        
        [OperationContract(IsOneWay = true)]
        void NotifyToUsers(NotificationToPerson Notification);
        
        [OperationContract(IsOneWay = true)]
        void NotifyForPermission(NotificationToPermission Notification);

        [OperationContract(IsOneWay = true)]
        void NotifyForRoles(NotificationToRole Notification);

        [OperationContract(IsOneWay = true)]
        void NotifyForPermissionItemGuid(NotificationToItemGuid Notification);

        [OperationContract(IsOneWay = true)]
        void NotifyForPermissionItemLong(NotificationToItemLong Notification);

        [OperationContract(IsOneWay = true)]
        void NotifyForPermissionItemVersionLong(NotificationToItemVersionLong Notification);

        [OperationContract(IsOneWay = true)]
        void NotifyForPermissionItemInt(NotificationToItemInt Notification);

        [OperationContract(IsOneWay = true)]
        void RemoveNotification(System.Guid NotificationID);

        [OperationContract(IsOneWay = true)]
        void RemoveNotificationForUser(System.Guid NotificationID, int PersonID);

        [OperationContract(IsOneWay = true)]
        void RemoveUserNotification(System.Guid UserNotificationID, int PersonID);

        [OperationContract(IsOneWay = true)]
        void ReadNotification(System.Guid NotificationID, int PersonID);

        [OperationContract(IsOneWay = true)]
        void ReadUserNotification(System.Guid UserNotificationID, int PersonID);

        [OperationContract(IsOneWay = true)]
        void ReadUserCommunityNotification(int CommunityID, int PersonID);

        [OperationContract(IsOneWay = true)]
        void RemoveUserCommunityNotification(int CommunityID, int PersonID);
    }
}