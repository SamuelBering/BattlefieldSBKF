using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class LocalPlayer : IPlayer
    {
        public OceanGridBoard OceanGridBoard { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TargetGridBoard TargetGridBoard { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IBattleShipProtocol BattleShipProtocol { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get; set; }

        public LocalPlayer(string name)
        {
            Name = name;
        }

        public void Connect(string host, int port, string localPlayerName)
        {
            throw new NotImplementedException();
        }

        public void Connect(int port, string localPlayerName)
        {
            throw new NotImplementedException();
        }

        
        public Command GetCommand()
        {
            throw new NotImplementedException();
        }

        public Response ExecuteCommand(Command command)
        {
            throw new NotImplementedException();
        }

        public Response ExecuteCommand(Commands cmd, params string[] parameters)
        {
            throw new NotImplementedException();
        }

        public Response GetResponse()
        {
            throw new NotImplementedException();
        }

        public Command ExecuteResponse(Responses resp, string parameter)
        {
            throw new NotImplementedException();
        }
    }
}
