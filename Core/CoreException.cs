using System;
using System.Runtime.Serialization;

namespace Core
{
    /// <summary>
    /// Base exception type for those are thrown by Core system for Core specific exceptions.
    /// </summary>
    [Serializable]
    public class CoreException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="CoreException"/> object.
        /// </summary>
        public CoreException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="CoreException"/> object.
        /// </summary>
        public CoreException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="CoreException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public CoreException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="CoreException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public CoreException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
