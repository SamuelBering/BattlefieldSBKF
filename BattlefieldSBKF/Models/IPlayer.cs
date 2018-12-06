using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public interface IPlayer
    {
        Command GetCommand();
        string ExecuteCommand(Command command);
        Command Connect(string host, int port);
        Command Connect(int port);
        OceanGridBoard OceanGridBoard { get; set; }
        TargetGridBoard TargetGridBoard { get; set; }
    }
}
