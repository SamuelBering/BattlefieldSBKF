using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class UnExpectedAnswerCodeException : Exception
    {
        public UnExpectedAnswerCodeException()
        {
        }

        public UnExpectedAnswerCodeException(string message)
            : base(message)
        {
        }

        public UnExpectedAnswerCodeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
