using System;
namespace lm.Comol.Core.Msmq.Services.Notifications.Service.Exceptions
{
    [Serializable]
    public class NotificationException : Exception 
    {
        public ExceptionType Type { get; set; }
        public NotificationException()
        {
        }

        public NotificationException(string message)
            : base(message)
        {
        }

        public NotificationException(string message, Exception inner)
            : base(message, inner)
        {
        }
        public NotificationException(Exception inner, ExceptionType type)
            : base(inner.Message, inner)
        {
        }
    }
}