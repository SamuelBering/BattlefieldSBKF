using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class BattleShipProtocol : IBattleShipProtocol
    {
        public string ParseCommand(Command command)
        {
            throw new NotImplementedException();
        }

        public Command ParseTcpCommand(string tcpCommand)
        {
            throw new NotImplementedException();
        }
    }
}
