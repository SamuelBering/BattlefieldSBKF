using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class UnExpectedCommandException : Exception
    {
        public UnExpectedCommandException()
        {
        }

        public UnExpectedCommandException(string message)
            : base(message)
        {
        }

        public UnExpectedCommandException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
