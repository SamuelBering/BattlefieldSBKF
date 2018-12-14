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

/*
  public Command ExecuteResponse(Response response, Command initialCommand, bool waitForCommand)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Response ExecuteCommand(Command command, bool waitForResponse, params Responses[] validResponses)
        {
            throw new NotImplementedException();
        }

        public Response ExecuteCommand(Commands cmd, bool waitForResponse, Responses[] validResponses, params string[] parameters)
        {
            throw new NotImplementedException();
        }

        public Command ExecuteResponse(Response response, bool waitForCommand, params Commands[] validCommands)
        {
            throw new NotImplementedException();
        }

        public Command ExecuteResponse(Responses resp, bool waitForCommand, string parameter, params Commands[] validCommands)
        {
            throw new NotImplementedException();
        }

          public Response ExecuteCommand(Commands cmd, bool waitForResponse, params string[] parameters)
        {
            throw new NotImplementedException();
        }

        public Response GetResponse(params Responses[] validResponses)
        {
            throw new NotImplementedException();
        }

     public bool Connect(string host, int port, string localPlayerName)
        {
            throw new NotImplementedException();
        }

        public bool Connect(int port, string localPlayerName)
        {
            throw new NotImplementedException();
        }


        public Command GetCommand(params Commands[] validCommands)
        {

            throw new NotImplementedException();

        }

     */
