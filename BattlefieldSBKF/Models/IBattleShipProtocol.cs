using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public interface IBattleShipProtocol
    {
        Command ParseTcpCommand(string tcpCommand);
        string ParseCommand(Command command);
    }
}
