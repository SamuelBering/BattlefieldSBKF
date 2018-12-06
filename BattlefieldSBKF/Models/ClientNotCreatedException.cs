using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class ClientNotCreatedException : Exception
    {
        public ClientNotCreatedException()
        {
        }

        public ClientNotCreatedException(string message)
            : base(message)
        {
        }

        public ClientNotCreatedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
