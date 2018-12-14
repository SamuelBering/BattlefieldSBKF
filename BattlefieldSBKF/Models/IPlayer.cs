using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public interface IPlayer 
    {
        void GetCommandOrResponse(out Command command, out Response response);
        Command ExecuteResponse(Response response, bool waitForCommand);
        Command ExecuteResponse(Responses resp, bool waitForCommand, string parameter);
        string Name { get; set; }
        IBattleShipProtocol BattleShipProtocol { get; set; }
        OceanGridBoard OceanGridBoard { get; set; }
        TargetGridBoard TargetGridBoard { get; set; }
        bool IsServer { get; set; }

    }
}
