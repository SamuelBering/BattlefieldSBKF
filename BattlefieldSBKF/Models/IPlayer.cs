using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public interface IPlayer
    {
        Command GetCommand();
        void GetCommandOrResponse(out Command command, out Response response);
        Response ExecuteCommand(Command command, bool waitForResponse);
        Response ExecuteCommand(Commands cmd, bool waitForResponse, params string[] parameters);
        Command ExecuteResponse(Response response, bool waitForCommand);
        Command ExecuteResponse(Responses resp, bool waitForCommand, string parameter);
        Response GetResponse();
        void Connect(string host, int port, string localPlayerName);
        void Connect(int port, string localPlayerName);

        string Name { get; set; }
        IBattleShipProtocol BattleShipProtocol { get; set; }
        OceanGridBoard OceanGridBoard { get; set; }
        TargetGridBoard TargetGridBoard { get; set; }
        bool IsServer { get; set; }

    }
}
