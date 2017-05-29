using System;
namespace lm.Comol.Core.Msmq.Services.Notifications.Service.Exceptions
{
    [Serializable]
    public enum ExceptionType
    {
        None = 0,
        DBaccess = 1,
        RemoteQueueUnavailable = 2,
        ConfigMising = 3,
        GenericError = 4,
        UnableToGetNhibernateSession = 5
    }
}