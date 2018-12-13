using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public enum Responses
    {
        Protocol,
        PlayerName,
        ClientStarts,
        HostStarts,
        Miss,
        HitCarrier,
        HitBattleship,
        HitDestroyer,
        HitSubmarine,
        HitPatrolBoat,
        SunkCarrier,
        SunkBattleship,
        SunkDestroyer,
        SunkSubmarine,
        SunkPatrolBoat,
        YouWin,
        ConnectionClosed,
        SyntaxError,
        SequenceError,
        Help
    }
}
