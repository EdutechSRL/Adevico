using System;
namespace lm.Comol.Core.Msmq.Services.Mail.Service
{
    [Serializable]
    public enum MailExceptionType
    {
        None = 0,
        MailSent = 1,
        InvalidAddress = 2,
        SMTPunavailable = 3,
        AuthenticationError = 4,
        NoDestinationAddress = 5,
        UnknownError = 6,
        NullMessage= 7
    }
}