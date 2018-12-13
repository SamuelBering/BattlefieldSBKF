using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class LocalPlayer : IPlayer
    {
        public OceanGridBoard OceanGridBoard { get; set; } = new OceanGridBoard(10);
        public TargetGridBoard TargetGridBoard { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IBattleShipProtocol BattleShipProtocol { get; set; } = new BattleShipProtocol();
        public string Name { get; set; }
        public bool IsServer { get; set; }

        public LocalPlayer()
        {
        }

        public LocalPlayer(string name, IBattleShipProtocol batprot)
        {
            Name = name;
            BattleShipProtocol = batprot;
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
            string input;
            while (true)
            {
                input = Console.ReadLine();

                var inputAsArray = input.Split(' ');

                var commandInput = inputAsArray[0];

                var commandComments = String.Join(string.Empty, inputAsArray, 1, inputAsArray.Length - 1);

                if (!String.IsNullOrEmpty(input))
                {
                    if (IsServer)
                    {

                        if (commandInput.ToLower() == "quit")
                        {
                            response = new Response(Responses.ConnectionClosed, null);
                            break;
                        }
                        else
                        {

                            if (commandInput.Length <= 3 && (int)commandInput.ToLower()[0] >= 97 &&
                                (int)commandInput.ToLower()[0] <= 106)
                            {
                                if (Int32.TryParse(commandInput.Substring(1, commandInput.Length - 1), out int result) &&
                                    result < 11 && result > 0)
                                {
                                    command = new Command(Commands.Fire, commandInput.Substring(0, 1).ToUpper(),
                                        result.ToString(), commandComments);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (input.ToLower() == "quit")
                        {
                            command = new Command(Commands.Quit, null);
                            break;
                        }
                        else
                        {
                            if (commandInput.Length <= 3 && (int)commandInput.ToLower()[0] >= 97 &&
                                (int)commandInput.ToLower()[0] <= 106)
                            {
                                if (Int32.TryParse(commandInput.Substring(1, commandInput.Length - 1), out int result) &&
                                    result < 11 && result > 0)
                                {
                                    command = new Command(Commands.Fire, commandInput.Substring(0, 1).ToUpper(),
                                        result.ToString(), commandComments);
                                    break;
                                }
                            }
                        }
                    }
                }


                Console.WriteLine("Ogiltigt kommando!");
            }
        }

        public Response ExecuteCommand(Command command, bool waitForResponse)
        {
            if (command.Cmd == Commands.Fire)
            {
                OceanGridBoard.ShowBoard();
                return OceanGridBoard.Fire(command.Parameters[0], command.Parameters[1]);
            }
            else
                throw new UnExpectedCommandException($"Expected command {Commands.Fire} but instead got: {command.Cmd}");
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
            return ExecuteResponse(new Response(resp, parameter), waitForCommand);
        }

        public Command ExecuteResponse(Response response, bool waitForCommand)
        {
            Command command = null;

            if (response.Resp == Responses.Miss)
            {
                Console.WriteLine("Du missade!");
            }
            else
                throw new NotImplementedException();

            if (waitForCommand)
                command = GetCommand();

            return command;
        }

        public Command ExecuteResponse(Response response, Command initialCommand, bool waitForCommand)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
