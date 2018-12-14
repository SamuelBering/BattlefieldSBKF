using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class LocalPlayer : ILocalPlayer
    {
        public OceanGridBoard OceanGridBoard { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TargetGridBoard TargetGridBoard { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IBattleShipProtocol BattleShipProtocol { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get; set; }
        public bool IsServer { get; set; }

        public LocalPlayer()
        {
        }

        public LocalPlayer(string name)
        {
            Name = name;
        }

        public Command GetCommand(params Commands[] validCommands)
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

                var commandComments = String.Join(string.Empty,inputAsArray, 1, inputAsArray.Length -1);
                

                if (IsServer)
                {

                    if (commandInput.ToLower() == "quit")
                    {
                        response = new Response(Responses.ConnectionClosed, null);
                        break;
                    }
                    else
                    {

                        if (commandInput.Length <= 3 && (int) commandInput.ToLower()[0] >= 97 &&
                            (int) commandInput.ToLower()[0] <= 106)
                        {
                            if (Int32.TryParse(commandInput.Substring(1, commandInput.Length-1), out int result) &&
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
                            if (Int32.TryParse(commandInput.Substring(1, commandInput.Length-1), out int result) &&
                                result < 11 && result > 0)
                            {
                                command = new Command(Commands.Fire, commandInput.Substring(0, 1).ToUpper(),
                                    result.ToString(), commandComments);
                                break;
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
                Console.WriteLine("(Träff destroyer)");
                return new Response(Responses.HitDestroyer, null);
            }
            else
                throw new UnExpectedCommandException($"Expected command {Commands.Fire} but instead got: {command.Cmd}");
        }

  

        public Command ExecuteResponse(Responses resp, bool waitForCommand, string parameter)
        {
            return ExecuteResponse(new Response(resp, parameter), waitForCommand);
        }

        public Command ExecuteResponse(Response response, bool waitForCommand)
        {
            Command command = null;

            if (response.Resp == Responses.HitDestroyer)
            {
                Console.WriteLine("Grattis: Du träffade destroyern!");
            }
            else
                throw new NotImplementedException();

            if (waitForCommand)
                command = GetCommand();

            return command;
        }

       
    }
}
