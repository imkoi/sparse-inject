using System;

namespace SparseInject
{
    public class SparseInjectException : Exception
    {
        public SparseInjectException(string message) : base(message)
        {
            
        }
        
        public SparseInjectException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}