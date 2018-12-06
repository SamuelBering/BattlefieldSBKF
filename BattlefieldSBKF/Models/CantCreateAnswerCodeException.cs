using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class CantCreateAnswerCodeException : Exception
    {
        public CantCreateAnswerCodeException()
        {
        }

        public CantCreateAnswerCodeException(string message)
            : base(message)
        {
        }

        public CantCreateAnswerCodeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
