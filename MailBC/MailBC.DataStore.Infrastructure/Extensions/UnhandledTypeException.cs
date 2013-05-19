using System;
using System.Runtime.Serialization;

namespace MailBC.DataStore.Infrastructure.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class UnhandledTypeException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public UnhandledTypeException() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public UnhandledTypeException(string message) : base(message) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public UnhandledTypeException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public UnhandledTypeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}