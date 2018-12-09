using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class CantCreateResponseException : Exception
    {
        public CantCreateResponseException()
        {
        }

        public CantCreateResponseException(string message)
            : base(message)
        {
        }

        public CantCreateResponseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
