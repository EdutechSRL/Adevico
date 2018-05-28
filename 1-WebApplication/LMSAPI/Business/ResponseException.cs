using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMSAPI
{
    /// <summary>
    /// 
    /// </summary>
    public class ResponseException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public int ServiceErrorCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ServiceErrorMessage { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public ResponseException() : base() {}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ResponseException(string message) : base(message) {}

        public ResponseException(string message, int serviceCode, string serviceMessage) : base(message)
        {
            ServiceErrorCode = serviceCode;
            ServiceErrorMessage = ServiceErrorMessage;
        }


    }
}