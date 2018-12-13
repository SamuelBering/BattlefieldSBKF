using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class BattleShipProtocol : IBattleShipProtocol
    {
        Dictionary<string, Commands> CommandsDict;
        Dictionary<string, Responses> ResponsesDict;
        Dictionary<Commands, string> TcpCommandsDict;
        Dictionary<Responses, string> TcpResponsesDict;
        Dictionary<string, int> YcordinateDict;

        public string ProtocolName { get; } = "BATTLESHIP/1.0";
        public Responses[] MissHitSunkWinResponses { get; set; }

        const string _clientStarts = "Client Starts";
        const string _hostsStarts = "Host Starts";
        const string _miss = "Miss!";
        const string _hitCarrier = "You hit my Carrier";
        const string _hitBattleShip = "You hit my Battleship";
        const string _hitDestroyer = "You hit my Destroyer";
        const string _hitSubmarine = "You hit my Submarine";
        const string _hitPatrolBoat = "You hit my Patrol Boat";
        const string _sunkCarrier = "You sunk my Carrier";
        const string _sunkBattleShip = "You sunk my Battleship";
        const string _sunkDestroyer = "You sunk my Destroyer";
        const string _sunkSubmarine = "You sunk my Submarine";
        const string _sunkPatrolBoat = "You sunk my Patrol Boat";
        const string _youWin = "You win!";
        const string _connectionClosed = "Connection closed";
        const string _syntaxError = "Syntax error";
        const string _sequenceError = "Sequence error";
        const string _help = "Not implemented";


        public BattleShipProtocol()
        {
            CreateCommandsDict();
            CreateResponsesDict();
            CreateYcordinateDict();
            CreateMissHitSunkWinResponses();
        }

        void CreateCommandsDict()
        {
            CommandsDict = new Dictionary<string, Commands>()
            {
                {"HELLO",Commands.Hello },
                {"START", Commands.Start },
                {"FIRE",Commands.Fire },
                 {"HELP", Commands.Help },
                {"QUIT",Commands.Quit }
            };

            TcpCommandsDict = new Dictionary<Commands, string>();
            foreach (var cmdIG in CommandsDict)
            {
                TcpCommandsDict.Add(cmdIG.Value, cmdIG.Key);
            }
        }

        void CreateResponsesDict()
        {
            ResponsesDict = new Dictionary<string, Responses>()
            {
                {"210", Responses.Protocol },
                {"220", Responses.PlayerName },
                {"221", Responses.ClientStarts },
                {"222", Responses.HostStarts },
                {"230", Responses.Miss },
                {"241", Responses.HitCarrier },
                {"242", Responses.HitBattleship },
                {"243", Responses.HitDestroyer },
                {"244", Responses.HitSubmarine },
                {"245", Responses.HitPatrolBoat },
                {"251", Responses.SunkCarrier },
                {"252", Responses.SunkBattleship },
                {"253", Responses.SunkDestroyer },
                {"254", Responses.SunkSubmarine },
                {"255", Responses.SunkPatrolBoat },
                {"260", Responses.YouWin },
                {"270", Responses.ConnectionClosed },
                {"280", Responses.Help },
                {"500", Responses.SyntaxError },
                {"501", Responses.SequenceError },
            };

            TcpResponsesDict = new Dictionary<Responses, string>();
            foreach (var respIG in ResponsesDict)
            {
                TcpResponsesDict.Add(respIG.Value, respIG.Key);
            }
        }

        void CreateYcordinateDict()
        {
            YcordinateDict = new Dictionary<string, int>()
            {
              {"A", 1 },
              {"B", 2 },
              {"C", 3 },
              {"D", 4 },
              {"E", 5 },
              {"F", 6 },
              {"G", 7 },
              {"H", 8 },
              {"I", 9 },
              {"J", 10 },
            };
        }

        void CreateMissHitSunkWinResponses()
        {
            MissHitSunkWinResponses = new Responses[]
            {
                Responses.Miss,
                Responses.HitCarrier,
                Responses.HitBattleship,
                Responses.HitDestroyer,
                Responses.HitSubmarine,
                Responses.HitPatrolBoat,
                Responses.SunkCarrier,
                Responses.SunkBattleship,
                Responses.SunkDestroyer,
                Responses.SunkSubmarine,
                Responses.SunkPatrolBoat,
                Responses.YouWin,
            };
        }

        public Response GetResponse(string tcpResponse)
        {
            var substrings = tcpResponse.Split(' ');

            var key = substrings[0];
            Response response = null;

            if (ResponsesDict.ContainsKey(key))
            {
                var responseEnum = ResponsesDict[key];

                switch (responseEnum)
                {
                    case Responses.Protocol:
                        if (substrings.Length == 2 && substrings[1] == this.ProtocolName)
                        {
                            response = new Response(responseEnum, substrings[1]);
                        }
                        else
                            throw new CantCreateResponseException($"Can't create response of input string {tcpResponse}. Parameter for protocol name is invalid.");
                        break;
                    case Responses.PlayerName:
                        if (substrings.Length > 1)
                        {
                            response = new Response(responseEnum, string.Join(' ', substrings.Skip(1).Take(substrings.Length - 1).ToArray()));
                        }
                        else
                            response = new Response(responseEnum, "");
                        break;
                    case Responses.HostStarts:
                        response = new Response(responseEnum, _hostsStarts);
                        break;
                    case Responses.ClientStarts:
                        response = new Response(responseEnum, _clientStarts);
                        break;
                    case Responses.Miss:
                        response = new Response(responseEnum, _miss);
                        break;
                    case Responses.HitCarrier:
                        response = new Response(responseEnum, _hitCarrier);
                        break;
                    case Responses.HitBattleship:
                        response = new Response(responseEnum, _hitBattleShip);
                        break;
                    case Responses.HitDestroyer:
                        response = new Response(responseEnum, _hitDestroyer);
                        break;
                    case Responses.HitSubmarine:
                        response = new Response(responseEnum, _hitSubmarine);
                        break;
                    case Responses.HitPatrolBoat:
                        response = new Response(responseEnum, _hitPatrolBoat);
                        break;
                    case Responses.SunkCarrier:
                        response = new Response(responseEnum, _sunkCarrier);
                        break;
                    case Responses.SunkBattleship:
                        response = new Response(responseEnum, _sunkBattleShip);
                        break;
                    case Responses.SunkDestroyer:
                        response = new Response(responseEnum, _sunkDestroyer);
                        break;
                    case Responses.SunkSubmarine:
                        response = new Response(responseEnum, _sunkSubmarine);
                        break;
                    case Responses.SunkPatrolBoat:
                        response = new Response(responseEnum, _sunkPatrolBoat);
                        break;
                    case Responses.YouWin:
                        response = new Response(responseEnum, _youWin);
                        break;
                    case Responses.ConnectionClosed:
                        response = new Response(responseEnum, _connectionClosed);
                        break;
                    case Responses.SyntaxError:
                        response = new Response(responseEnum, _syntaxError);
                        break;
                    case Responses.SequenceError:
                        response = new Response(responseEnum, _sequenceError);
                        break;

                    default:
                        throw new NotImplementedException($"Can't create response. Response: {responseEnum} is not implemented.");
                }

                return response;
            }
            else
            {
                throw new CantCreateResponseException($"Can't create response of input string {tcpResponse}. Key: {key} doesn't exists.");
            }


        }

        public string GetTcpResponse(Response response)
        {
            string tcpResponse = null;


            switch (response.Resp)
            {
                case Responses.Protocol:
                    tcpResponse = $"{TcpResponsesDict[response.Resp]} {ProtocolName}";
                    break;

                case Responses.PlayerName:
                    if (response.Parameter != null && response.Parameter.Length > 0)
                    {
                        tcpResponse = $"{TcpResponsesDict[response.Resp]} {response.Parameter}";
                    }
                    else
                        throw new CantCreateResponseException($"Can't create tcp response of {response.Resp}. Parameter: {response.Parameter} for Name is invalid.");
                    break;

                case Responses.HostStarts:
                    tcpResponse = $"{TcpResponsesDict[response.Resp]} {_hostsStarts}";
                    break;

                case Responses.ClientStarts:
                    tcpResponse = $"{TcpResponsesDict[response.Resp]} {_clientStarts}";
                    break;

                case Responses.Miss:
                    tcpResponse = $"{TcpResponsesDict[response.Resp]} {_miss}";
                    break;

                case Responses.HitCarrier:
                    tcpResponse = $"{TcpResponsesDict[response.Resp]} {_hitCarrier}";
                    break;

                case Responses.HitBattleship:
                    tcpResponse = $"{TcpResponsesDict[response.Resp]} {_hitBattleShip}";
                    break;

                case Responses.HitDestroyer:
                    tcpResponse = $"{TcpResponsesDict[response.Resp]} {_hitDestroyer}";
                    break;

                case Responses.HitSubmarine:
                    tcpResponse = $"{TcpResponsesDict[response.Resp]} {_hitSubmarine}";
                    break;

                case Responses.HitPatrolBoat:
                    tcpResponse = $"{TcpResponsesDict[response.Resp]} {_hitPatrolBoat}";
                    break;

                case Responses.SunkCarrier:
                    tcpResponse = $"{TcpResponsesDict[response.Resp]} {_sunkCarrier}";
                    break;

                case Responses.SunkBattleship:
                    tcpResponse = $"{TcpResponsesDict[response.Resp]} {_sunkBattleShip}";
                    break;

                case Responses.SunkDestroyer:
                    tcpResponse = $"{TcpResponsesDict[response.Resp]} {_sunkDestroyer}";
                    break;

                case Responses.SunkSubmarine:
                    tcpResponse = $"{TcpResponsesDict[response.Resp]} {_sunkSubmarine}";
                    break;

                case Responses.SunkPatrolBoat:
                    tcpResponse = $"{TcpResponsesDict[response.Resp]} {_sunkPatrolBoat}";
                    break;

                case Responses.YouWin:
                    tcpResponse = $"{TcpResponsesDict[response.Resp]} {_youWin}";
                    break;

                case Responses.ConnectionClosed:
                    tcpResponse = $"{TcpResponsesDict[response.Resp]} {_connectionClosed}";
                    break;

                case Responses.SyntaxError:
                    tcpResponse = $"{TcpResponsesDict[response.Resp]} {_syntaxError}";
                    break;

                case Responses.SequenceError:
                    tcpResponse = $"{TcpResponsesDict[response.Resp]} {_sequenceError}";
                    break;

                case Responses.Help:
                    tcpResponse = $"{TcpResponsesDict[response.Resp]} {_help}";
                    break;

                default:
                    throw new CantCreateResponseException($"Can't create tcp response of {response.Resp} because it's not implemented.");

            }

            return tcpResponse;
        }

        public Command GetCommand(string tcpCommand)
        {
            var substrings = tcpCommand.Split(' ');

            var key = substrings[0].ToUpper();
            Command command = null;

            if (CommandsDict.ContainsKey(key))
            {
                var commandEnum = CommandsDict[key];

                switch (commandEnum)
                {
                    case Commands.Hello:
                        if (substrings.Length > 1 && substrings[1].Length > 0)
                        {
                            command = new Command(commandEnum, substrings.Skip(1).Take(substrings.Length - 1).ToArray());
                        }
                        else
                            throw new CantCreateCommandException($"Can't create command of input string {tcpCommand}. Parameter for Name is invalid.");
                        break;
                    case Commands.Start:
                        if (substrings.Length == 1)
                        {
                            command = new Command(commandEnum, null);
                        }
                        else
                            throw new CantCreateCommandException($"Can't create command of input string {tcpCommand}. No parameter is allowed for this command.");
                        break;
                    case Commands.Fire:
                        if (substrings.Length > 1)
                        {
                            string yCordinate = substrings[1].Substring(0, 1), xCordinate = substrings[1].Substring(1);

                            bool xCordIsValidNumber = int.TryParse(xCordinate, out int xCordInt);

                            if (YcordinateDict.ContainsKey(yCordinate) && (xCordIsValidNumber && xCordInt >= 1 && xCordInt <= 10))
                            {
                                var parameters = new List<string>
                                {
                                    yCordinate,
                                    xCordinate
                                };
                                if (substrings.Length > 2)
                                    parameters.AddRange(substrings.Skip(2).Take(substrings.Length - 2).ToList());

                                command = new Command(commandEnum, parameters.ToArray());
                            }
                            else
                                throw new CantCreateCommandException($"Can't create tcp command of command {command.Cmd}. Parameters for cordinates are invalid.");
                        }
                        else
                            throw new CantCreateCommandException($"Can't create command of input string {tcpCommand}. Must be at least one parameter for this command.");
                        break;
                    case Commands.Quit:
                        command = new Command(commandEnum, null);
                        break;
                    case Commands.Help:
                        command = new Command(commandEnum, null);
                        break;
                    default:
                        throw new NotImplementedException($"Can't create command. Command: {command.Cmd} is not implemented.");

                }

                return command;
            }
            else
            {
                throw new CantCreateCommandException($"Can't create command of input string {tcpCommand}. Key: {key} doesn't exists.");
            }

        }

        public string GetTcpCommand(Command command)
        {

            string tcpCommand = null;


            switch (command.Cmd)
            {
                case Commands.Hello:
                    if (command.Parameters.Length > 0 && command.Parameters[0].Length > 0)
                    {
                        tcpCommand = $"{TcpCommandsDict[command.Cmd]} {string.Join(' ', command.Parameters)}";
                    }
                    else
                        throw new CantCreateCommandException($"Can't create tcp tcp command of command {command.Cmd}. Invalid parameters.");
                    break;
                case Commands.Start:
                    if (command.Parameters == null)
                    {
                        tcpCommand = $"{TcpCommandsDict[command.Cmd]}";
                    }
                    else
                        throw new CantCreateCommandException($"Can't create tcp command of command {command.Cmd}. No parameter is allowed for this command.");
                    break;
                case Commands.Fire:
                    if (command.Parameters.Length == 2 || command.Parameters.Length == 3)
                    {
                        string yCordinate = command.Parameters[0], message = command.Parameters.Length == 3 ? command.Parameters[2] : null;
                        bool xCordIsValidNumber = int.TryParse(command.Parameters[1], out int xCordinate);

                        if (YcordinateDict.ContainsKey(yCordinate) && (xCordIsValidNumber && xCordinate >= 1 && xCordinate <= 10))
                        {
                            tcpCommand = $"{TcpCommandsDict[command.Cmd]} {yCordinate}{xCordinate}";
                            tcpCommand += message != null ? " " + message : "";
                        }
                        else
                            throw new CantCreateCommandException($"Can't create tcp command of command {command.Cmd}. Parameters for cordinates are invalid.");

                    }
                    else
                        throw new CantCreateCommandException($"Can't create tcp command of command {command.Cmd}. Number of parameters must be two or three.");
                    break;
                case Commands.Quit:
                    tcpCommand = $"{TcpCommandsDict[command.Cmd]}";
                    break;
                default:
                    throw new NotImplementedException($"Can't create tcp command. Command: {command.Cmd} is not implemented.");
            }

            return tcpCommand;


        }


    }
}
