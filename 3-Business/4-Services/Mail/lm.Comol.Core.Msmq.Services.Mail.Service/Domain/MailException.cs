using System;
namespace lm.Comol.Core.Msmq.Services.Mail.Service
{
    [Serializable]
    public class MailException : Exception 
    {
        public MailExceptionType Type { get; set; }
        public MailException()
        {
        }

        public MailException(string message)
            : base(message)
        {
        }

        public MailException(string message, Exception inner)
            : base(message, inner)
        {
        }
        public MailException(Exception inner, MailExceptionType type)
            : base(inner.Message, inner)
        {
        }

    }
}