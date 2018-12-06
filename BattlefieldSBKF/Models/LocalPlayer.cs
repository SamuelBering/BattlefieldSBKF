using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class LocalPlayer : IPlayer
    {
        public OceanGridBoard OceanGridBoard { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TargetGridBoard TargetGridBoard { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string ExecuteCommand(Command command)
        {
            throw new NotImplementedException();
        }

        public Command GetCommand()
        {
            throw new NotImplementedException();
        }
    }
}
