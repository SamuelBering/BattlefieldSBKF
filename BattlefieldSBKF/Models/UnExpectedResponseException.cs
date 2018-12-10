using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class UnExpectedResponseException : Exception
    {
        public UnExpectedResponseException()
        {
        }

        public UnExpectedResponseException(string message)
            : base(message)
        {
        }

        public UnExpectedResponseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
