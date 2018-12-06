using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class CantCreateCommandException : Exception
    {
        public CantCreateCommandException()
        {
        }

        public CantCreateCommandException(string message)
            : base(message)
        {
        }

        public CantCreateCommandException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
