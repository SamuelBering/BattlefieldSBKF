using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class BattleShipGameEngine
    {
        IPlayer _localPlayer;
        IPlayer _remotePlayer;

        public BattleShipGameEngine(IPlayer localPlayer, IPlayer remotePlayer)
        {
            _localPlayer = localPlayer;
            _remotePlayer = remotePlayer;
        }
    }
}
