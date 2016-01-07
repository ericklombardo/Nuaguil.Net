using System;

namespace Nuaguil.NhContrib
{
    public class PersistenceException : Exception
    {


        public PersistenceException(string message)
            : base(message)
        {
        }
        public PersistenceException(string message,Exception exc)
            : base(message,exc)
        {
        }
    }
}
