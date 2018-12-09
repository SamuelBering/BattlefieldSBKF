using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public interface IBattleShipProtocol
    {
        string ProtocolName { get; }

        Command GetCommand(string tcpCommand);
        string GetTcpCommand(Command command);

        Response GetResponse(string tcpResponse);
        string GetTcpResponse(Response response);

    }
}
