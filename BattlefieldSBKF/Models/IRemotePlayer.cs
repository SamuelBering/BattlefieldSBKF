using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public interface IRemotePlayer : IPlayer, IDisposable
    {
        Command GetCommand(params Commands[] validCommands);
        Response ExecuteCommand(Command command, bool waitForResponse, params Responses[] validResponses);
        Response ExecuteCommand(Commands cmd, bool waitForResponse, Responses[] validResponses, params string[] parameters);
        Command ExecuteResponse(Response response, bool waitForCommand, params Commands[] validCommands);
        Command ExecuteResponse(Responses resp, bool waitForCommand, string parameter, params Commands[] validCommands);
        Response GetResponse(params Responses[] validResponses);
        bool Connect(string host, int port, string localPlayerName);
        bool Connect(int port, string localPlayerName);

    }
}
