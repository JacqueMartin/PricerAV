using System;
using System.Runtime.Serialization;

namespace PricerAV
{
    [Serializable]
    internal class InvalidHistoricalDataException : Exception
    {
        public InvalidHistoricalDataException()
        {
        }

        public InvalidHistoricalDataException(string message) : base(message)
        {
        }

        public InvalidHistoricalDataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidHistoricalDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}