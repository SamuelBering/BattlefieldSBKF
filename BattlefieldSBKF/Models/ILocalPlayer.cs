using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public interface ILocalPlayer : IPlayer
    {
        Response ExecuteCommand(Command command, bool waitForResponse);
        Command GetCommand(params Commands[] validCommands);
    }
}
