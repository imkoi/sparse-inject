using System;

namespace SparseInject
{
    public class SparseInjectException : Exception
    {
        public SparseInjectException(string message) : base(message)
        {
            
        }

        public SparseInjectException()
        {
            throw new NotImplementedException();
        }
    }
}