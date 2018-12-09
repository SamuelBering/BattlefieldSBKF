using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public interface IPlayer
    {
        Command GetCommand();
        string ExecuteCommand(Command command);
        void Connect(string host, int port, string localPlayerName);
        void Connect(int port, string localPlayerName);

        string Name { get; set; }
        IBattleShipProtocol BattleShipProtocol { get; set; }
        OceanGridBoard OceanGridBoard { get; set; }
        TargetGridBoard TargetGridBoard { get; set; }
    }
}
