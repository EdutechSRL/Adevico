using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Notification.Core.DataLayer
{
    public class DatalayerTransactionException : ApplicationException
    {
        public DatalayerTransactionException()
        {
        }

        public DatalayerTransactionException(string message)
            : base(message)
        {
        }

        public DatalayerTransactionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
