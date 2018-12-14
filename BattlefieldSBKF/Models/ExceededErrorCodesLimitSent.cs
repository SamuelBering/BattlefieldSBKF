using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class ExceededErrorCodesLimitSent : Exception
    {
        public ExceededErrorCodesLimitSent()
        {
        }

        public ExceededErrorCodesLimitSent(string message)
            : base(message)
        {
        }

        public ExceededErrorCodesLimitSent(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
