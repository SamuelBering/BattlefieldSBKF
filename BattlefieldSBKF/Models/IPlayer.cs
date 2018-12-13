using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public interface IPlayer : IDisposable
    {
        Command GetCommand(params Commands[] validCommands);
        void GetCommandOrResponse(out Command command, out Response response);
        Response ExecuteCommand(Command command, bool waitForResponse, params Responses[] validResponses);
        Response ExecuteCommand(Commands cmd, bool waitForResponse, Responses[] validResponses, params string[] parameters);
        Command ExecuteResponse(Response response, bool waitForCommand);
        Command ExecuteResponse(Response response, Command initialCommand, bool waitForCommand);
        Command ExecuteResponse(Responses resp, bool waitForCommand, string parameter);
        Response GetResponse(params Responses[] validResponses);
        bool Connect(string host, int port, string localPlayerName);
        bool Connect(int port, string localPlayerName);

        string Name { get; set; }
        IBattleShipProtocol BattleShipProtocol { get; set; }
        OceanGridBoard OceanGridBoard { get; set; }
        TargetGridBoard TargetGridBoard { get; set; }
        bool IsServer { get; set; }

    }
}
