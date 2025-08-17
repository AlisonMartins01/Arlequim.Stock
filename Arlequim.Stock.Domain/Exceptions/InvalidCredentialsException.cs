using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arlequim.Stock.Domain.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException()
            : base("Invalid credentials.")
        {
        }

        public InvalidCredentialsException(string message)
            : base(message)
        {
        }

        public InvalidCredentialsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
