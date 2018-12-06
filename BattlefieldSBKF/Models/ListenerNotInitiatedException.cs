using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class ListenerNotInitiatedException: Exception
    {
        public ListenerNotInitiatedException()
        {
        }

        public ListenerNotInitiatedException(string message)
            : base(message)
        {
        }

        public ListenerNotInitiatedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
