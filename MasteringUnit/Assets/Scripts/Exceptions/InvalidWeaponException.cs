using System;

namespace Exceptions
{
    public class InvalidWeaponException : Exception
    {
        public InvalidWeaponException(string message) : base(message)
        {
        }

        protected InvalidWeaponException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}