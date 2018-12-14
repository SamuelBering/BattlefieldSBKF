using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public interface ILocalPlayer : IPlayer
    {
        Response ExecuteCommand(Command command, bool waitForResponse, ref bool endGame);
        Command GetCommand(params Commands[] validCommands);
        Command ExecuteResponse(Response response, Command initialCommand, bool waitForCommand, ref bool endGame);
    }
}
