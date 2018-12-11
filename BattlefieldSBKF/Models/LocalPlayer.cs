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
        public bool IsServer { get; set; }

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

        public void GetCommandOrResponse(out Command command, out Response response)
        {
            command = null;
            response = null;

            Console.Write("Din tur: (tex A1 eller quit för att avsluta)");
            var input = Console.ReadLine();

            if (IsServer)
            {
                if (input == "quit")
                {
                    response = new Response(Responses.ConnectionClosed, null);
                }
                else
                    command = new Command(Commands.Fire, input.Substring(0, 1), input.Substring(1));
            }
            else
            {
                if (input == "quit")
                {
                    command = new Command(Commands.Quit, null);
                }
                else
                    command = new Command(Commands.Fire, input.Substring(0, 1), input.Substring(1));
            }

        }

        public Response ExecuteCommand(Command command, bool waitForResponse)
        {
            throw new NotImplementedException();
        }

        public Response ExecuteCommand(Commands cmd, bool waitForResponse, params string[] parameters)
        {
            throw new NotImplementedException();
        }

        public Response GetResponse()
        {
            throw new NotImplementedException();
        }



        public Command ExecuteResponse(Responses resp, bool waitForCommand, string parameter)
        {
            throw new NotImplementedException();
        }

        public Command ExecuteResponse(Response response, bool waitForCommand)
        {
            throw new NotImplementedException();
        }
    }
}
