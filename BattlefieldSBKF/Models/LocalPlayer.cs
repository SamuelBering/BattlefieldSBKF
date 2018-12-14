using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class LocalPlayer : ILocalPlayer
    {
        public OceanGridBoard OceanGridBoard { get; set; } = new OceanGridBoard(10, new BattleShipProtocol());
        public TargetGridBoard TargetGridBoard { get; set;} = new TargetGridBoard(10, new BattleShipProtocol());
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

        public Command GetCommand(params Commands[] validCommands)
        {

            throw new NotImplementedException();

        }

        public void GetCommandOrResponse(out Command command, out Response response)
        {
            command = null;
            response = null;

            OceanGridBoard.ShowBoard();
            TargetGridBoard.ShowBoard();
            Console.Write("Din tur (tex A1 eller quit för att avsluta): ");
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

        public Response ExecuteCommand(Command command, bool waitForResponse, ref bool endGame)
        {
            if (command.Cmd == Commands.Fire)
            {
                var response = OceanGridBoard.Fire(command.Parameters[0], command.Parameters[1]);

                if (response.Resp == Responses.YouWin)
                {
                    Console.WriteLine($"{Name}....du är en sån jäddra..LOOSEER!!!");
                    endGame = true;
                }

                return response;
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
            
            switch (response.Resp)
            {
                case Responses.HitDestroyer:
                    Console.WriteLine("Grattis: Du träffade destroyern!");
                    break;
                case Responses.HitBattleship:
                    Console.WriteLine("Grattis: Du träffade battleshipet!");
                    break;
                case Responses.HitCarrier:
                    Console.WriteLine("Grattis: Du träffade carriern!");
                    break;
                case Responses.HitSubmarine:
                    Console.WriteLine("Grattis: Du träffade submarinen!");
                    break;
                case Responses.HitPatrolBoat:
                    Console.WriteLine("Grattis: Du träffade patrolboaten!");
                    break;
                case Responses.SunkDestroyer:
                    Console.WriteLine("Grattis: Du sänkte destroyern!");
                    break;
                case Responses.SunkBattleship:
                    Console.WriteLine("Bravo: Du sänkte battleshipet!");
                    break;
                case Responses.SunkCarrier:
                    Console.WriteLine("Bravo: Du sänkte carriern!");
                    break;
                case Responses.SunkSubmarine:
                    Console.WriteLine("Bravo: Du sänkte submarinen!");
                    break;
                case Responses.SunkPatrolBoat:
                    Console.WriteLine("Bravo: Du sänkte patrolboaten!");
                    break;
                case Responses.Miss:
                    Console.WriteLine("Synd: Du missade...");
                    break;
                case Responses.YouWin:
                    Console.WriteLine("Jättebra!!: Du vann");
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (waitForCommand)
                command = GetCommand();

            return command;
        }

        public Command ExecuteResponse(Response response, Command initialCommand, bool waitForCommand, ref bool endGame)
        {
            Command command = null;

            if ((int)response.Resp == 4)
            {
                TargetGridBoard.MarkShot(initialCommand.Parameters[0], initialCommand.Parameters[1], hit: false);
            }
            else if ((int)response.Resp >= 5 && (int)response.Resp <= 15)
            {
                TargetGridBoard.MarkShot(initialCommand.Parameters[0], initialCommand.Parameters[1], hit: true);
            }

            OceanGridBoard.ShowBoard();

            TargetGridBoard.ShowBoard();

            switch (response.Resp)
            {
                case Responses.HitDestroyer:
                    Console.WriteLine("Grattis: Du träffade destroyern!");
                    break;
                case Responses.HitBattleship:
                    Console.WriteLine("Grattis: Du träffade battleshipet!");
                    break;
                case Responses.HitCarrier:
                    Console.WriteLine("Grattis: Du träffade carriern!");
                    break;
                case Responses.HitSubmarine:
                    Console.WriteLine("Grattis: Du träffade submarinen!");
                    break;
                case Responses.HitPatrolBoat:
                    Console.WriteLine("Grattis: Du träffade patrolboaten!");
                    break;
                case Responses.SunkDestroyer:
                    Console.WriteLine("Grattis: Du sänkte destroyern!");
                    break;
                case Responses.SunkBattleship:
                    Console.WriteLine("Bravo: Du sänkte battleshipet!");
                    break;
                case Responses.SunkCarrier:
                    Console.WriteLine("Bravo: Du sänkte carriern!");
                    break;
                case Responses.SunkSubmarine:
                    Console.WriteLine("Bravo: Du sänkte submarinen!");
                    break;
                case Responses.SunkPatrolBoat:
                    Console.WriteLine("Bravo: Du sänkte patrolboaten!");
                    break;
                case Responses.Miss:
                    Console.WriteLine("Synd: Du missade...");
                    break;
                case Responses.YouWin:
                    endGame = true;
                    Console.WriteLine($"Jättebra!!: Du vann {Name} ***");
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (waitForCommand)
                command = GetCommand();

            return command;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
