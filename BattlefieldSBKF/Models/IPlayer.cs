using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public interface IPlayer
    {
        Command GetCommand();
        string ExecuteCommand(Command command);
        OceanGridBoard OceanGridBoard { get; set; }
        TargetGridBoard TargetGridBoard { get; set; }
    }
}
