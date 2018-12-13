using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public interface IBattleShipProtocol
    {
        string ProtocolName { get; }
        Responses[] MissHitSunkWinResponses { get; set; }

        Command GetCommand(string tcpCommand);
        string GetTcpCommand(Command command);

        Response GetResponse(string tcpResponse);
        string GetTcpResponse(Response response);

    }
}
