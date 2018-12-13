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
        public Dictionary<string, int> YcoordinateDict { get => YcordinateDict; set => YcordinateDict = value; }

        string _clientStarts = "Client Starts";
        string _hostsStarts = "Host Starts";
        string _miss = "Miss!";
        string _connectionClosed = "Connection closed";


        public BattleShipProtocol()
        {
            CreateCommandsDict();
            CreateResponsesDict();
            CreateYcordinateDict();
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
                        if (substrings.Length > 1 && substrings[1].Length > 0)
                        {
                            response = new Response(responseEnum, string.Join(' ', substrings.Skip(1).Take(substrings.Length - 1).ToArray()));
                        }
                        else
                            throw new CantCreateResponseException($"Can't create response of input string {tcpResponse}. Parameter for Name is invalid.");
                        break;
                    case Responses.HostStarts:
                        string info = null;
                        if (substrings.Length > 1)
                            info = string.Join(' ', substrings.Skip(1).Take(substrings.Length - 1).ToArray());
                        response = new Response(responseEnum, info);
                        break;
                    case Responses.Miss:
                        response = new Response(responseEnum, _miss);
                        break;
                    case Responses.ConnectionClosed:
                        response = new Response(responseEnum, _connectionClosed);
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
                    if (response.Parameter == null || response.Parameter.ToLower() == ProtocolName.ToLower())
                    {
                        tcpResponse = $"{TcpResponsesDict[response.Resp]} {ProtocolName}";
                    }
                    else
                        throw new CantCreateResponseException($"Can't create tcp response of {response.Resp}. Parameter: {response.Parameter} for protocol name is invalid.");
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
                    if (response.Parameter == null || response.Parameter.ToLower() == _hostsStarts.ToLower())
                    {
                        tcpResponse = $"{TcpResponsesDict[response.Resp]} {_hostsStarts}";
                    }
                    else
                        throw new CantCreateResponseException($"Can't create tcp response of {response.Resp}. Parameter: {response.Parameter} is invalid.");
                    break;

                case Responses.ClientStarts:
                    if (response.Parameter == null || response.Parameter.ToLower() == _clientStarts.ToLower())
                    {
                        tcpResponse = $"{TcpResponsesDict[response.Resp]} {_clientStarts}";
                    }
                    else
                        throw new CantCreateResponseException($"Can't create tcp response of {response.Resp}. Parameter: {response.Parameter} is invalid.");
                    break;

                case Responses.Miss:
                    if (response.Parameter == null || response.Parameter.ToLower() == _miss.ToLower())
                    {
                        tcpResponse = $"{TcpResponsesDict[response.Resp]} {_miss}";
                    }
                    else
                        throw new CantCreateResponseException($"Can't create tcp response of {response.Resp}. Parameter: {response.Parameter} is invalid.");
                    break;
                case Responses.ConnectionClosed:
                    tcpResponse = $"{TcpResponsesDict[response.Resp]} {_connectionClosed}";
                    break;

                default:
                    throw new CantCreateResponseException($"Can't create tcp response of {response.Resp} because it's not implemented.");

            }

            return tcpResponse;
        }

        public Command GetCommand(string tcpCommand)
        {
            var substrings = tcpCommand.Split(' ');

            var key = substrings[0];
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
