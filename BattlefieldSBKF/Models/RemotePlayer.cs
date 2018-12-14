using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class RemotePlayer : IRemotePlayer
    {

        WrappedStreamReader _reader;
        WrappedStreamWriter _writer;
        TcpClient _client;
        NetworkStream _networkStream;
        TcpListener _listener;

        public OceanGridBoard OceanGridBoard { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TargetGridBoard TargetGridBoard { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IBattleShipProtocol BattleShipProtocol { get; set; }
        public string Name { get; set; }
        public bool IsServer { get; set; }


        public RemotePlayer(IBattleShipProtocol battleShipProtocol)
        {
            BattleShipProtocol = battleShipProtocol;
        }

        TcpListener StartListen(int port)
        {

            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            return listener;
        }

        public bool Connect(string host, int port, string localPlayerName)
        {
            try
            {
                _client = new TcpClient(host, port);
            }
            catch (SocketException ex)
            {
                throw new ClientNotCreatedException($"Kan ej starta förbindelse. Port {port} troligtvis upptagen");
            }
            _networkStream = _client.GetStream();
            _reader = new WrappedStreamReader(new StreamReader(_networkStream, Encoding.UTF8), IsServer);
            _writer = new WrappedStreamWriter(new StreamWriter(_networkStream, Encoding.UTF8) { AutoFlush = true }, IsServer);

            Response response = GetResponse(exitImmediatelyOnError: true, validResponses: Responses.Protocol);
            if (response.Resp == Responses.ConnectionClosed)
                return false;

            response = ExecuteCommand(Commands.Helo, waitForResponse: true,
                validResponses: new Responses[] { Responses.PlayerName, Responses.SyntaxError }, parameters: localPlayerName);

            if (response.Resp == Responses.SyntaxError)
            {
                response = ExecuteCommand(Commands.Hello, waitForResponse: true,
                validResponses: new Responses[] { Responses.PlayerName }, parameters: localPlayerName);
            }

            if (response.Resp == Responses.ConnectionClosed)
                return false;

            this.Name = response.Parameter;
            Console.WriteLine($"Du spelar mot: {this.Name}\tIpaddress: {_client.Client.RemoteEndPoint}");
            response = ExecuteCommand(Commands.Start, waitForResponse: false, validResponses: null, parameters: null);
            return true;
        }


        public bool Connect(int port, string localPlayerName)
        {

            try
            {
                _listener = StartListen(port);
            }
            catch (SocketException ex)
            {
                throw new ListenerNotInitiatedException($"Kan ej starta förbindelse. Port {port} troligtvis upptagen");
            }

            _client = _listener.AcceptTcpClient();
            _networkStream = _client.GetStream();
            _reader = new WrappedStreamReader(new StreamReader(_networkStream, Encoding.UTF8), IsServer);
            _writer = new WrappedStreamWriter(new StreamWriter(_networkStream, Encoding.UTF8) { AutoFlush = true }, IsServer);


            Command command = ExecuteResponse(Responses.Protocol, waitForCommand: true,
                            parameter: null, validCommands: new Commands[] { Commands.Hello, Commands.Helo });
            if (command.Cmd == Commands.Quit)
                return false;

            this.Name = string.Join(' ', command.Parameters);

            Console.WriteLine($"Du spelar mot: {this.Name}\tIpaddress: {_client.Client.RemoteEndPoint}");

            command = ExecuteResponse(Responses.PlayerName, waitForCommand: true, parameter: localPlayerName, validCommands: Commands.Start);
            if (command.Cmd == Commands.Quit)
                return false;
            return true;
        }

        public Response ExecuteCommand(Command command, bool waitForResponse, params Responses[] validResponses)
        {
            Response response = null;

            var tcpCommand = BattleShipProtocol.GetTcpCommand(command);
            _writer.WriteLine(tcpCommand);

            if (waitForResponse)
                response = GetResponse(false, validResponses);

            return response;

        }

        public Response ExecuteCommand(Commands cmd, bool waitForResponse, Responses[] validResponses, params string[] parameters)
        {
            Command command = new Command(cmd, parameters);
            return ExecuteCommand(command, waitForResponse, validResponses);
        }


        public Command GetCommand(params Commands[] validCommands)
        {
            Command command = null;
            Response response = null;

            int errorCount = 0;
            while (true)
            {
                if (errorCount > 3)
                {
                    if (!IsServer)
                        ExecuteResponse(Responses.ConnectionClosed, false);

                    throw new ExceededErrorCodesLimitSent($"Number of error codes sent exceeded maxium(3).");
                }

                try
                {

                    GetCommandOrResponse(out command, out response);

                    if (command != null)
                    {
                        if (command.Cmd == Commands.Quit && !IsServer)
                        {
                            ExecuteResponse(Responses.ConnectionClosed, false, null);
                            return command;
                        }
                        else if (command.Cmd == Commands.Help)
                        {
                            ExecuteResponse(Responses.Help, false);
                            continue;
                        }

                        bool isValidCommand = false;

                        foreach (Commands validCommand in validCommands)
                        {
                            if (command.Cmd == validCommand)
                                isValidCommand = true;
                        }

                        if (isValidCommand)
                            return command;
                        else
                        {
                            ExecuteResponse(Responses.SequenceError, false);
                            errorCount++;
                        }
                    }
                    else
                    {
                        if (response.Resp == Responses.ConnectionClosed && IsServer)
                        {
                            command = new Command(Commands.Quit, null);
                            return command;
                        }
                        else
                        {
                            ExecuteResponse(Responses.SequenceError, false);
                            errorCount++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex is CantCreateCommandException || ex is CantCreateResponseException)
                    {
                        ExecuteResponse(Responses.SyntaxError, false);
                        errorCount++;
                    }
                    else
                        throw;
                }

            }
        }

        public void GetCommandOrResponse(out Command command, out Response response)
        {
            command = null;
            response = null;

            var tcpString = _reader.ReadLine();
            try
            {
                command = BattleShipProtocol.GetCommand(tcpString);
            }
            catch
            {
                response = BattleShipProtocol.GetResponse(tcpString);
            }
        }


        public Response GetResponse(bool exitImmediatelyOnError, params Responses[] validResponses)
        {
            Command command = null;
            Response response = null;

            int errorCount = 0;
            while (true)
            {
                if (errorCount > 3)
                {
                    if (!IsServer)
                        ExecuteResponse(Responses.ConnectionClosed, false);

                    throw new ExceededErrorCodesLimitSent($"Number of error codes sent exceeded maxium(3).");
                }

                try
                {

                    GetCommandOrResponse(out command, out response);

                    if (response != null)
                    {
                        if (response.Resp == Responses.ConnectionClosed && IsServer)
                        {
                            return response;
                        }

                        bool isValidResponse = false;

                        foreach (Responses validResponse in validResponses)
                        {
                            if (response.Resp == validResponse)
                                isValidResponse = true;
                        }

                        if (isValidResponse)
                            return response;
                        else
                        {
                            if (exitImmediatelyOnError)
                                throw new UnExpectedResponseException($"Received an invalid response: {response.Resp}.");

                            ExecuteResponse(Responses.SequenceError, false);
                            errorCount++;
                        }
                    }
                    else
                    {
                        if (command.Cmd == Commands.Quit && !IsServer)
                        {
                            response = new Response(Responses.ConnectionClosed, null);
                            ExecuteResponse(response, false);
                            return response;
                        }
                        else if (command.Cmd == Commands.Help)
                        {
                            ExecuteResponse(Responses.Help, false);
                            continue;
                        }
                        else
                        {
                            if (exitImmediatelyOnError)
                                throw new UnExpectedCommandException($"Received an invalid command: {command.Cmd}.");

                            ExecuteResponse(Responses.SequenceError, false);
                            errorCount++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex is CantCreateCommandException || ex is CantCreateResponseException)
                    {
                        if (exitImmediatelyOnError)
                            throw new UnExpectedResponseException($"Received an invalid response from server.");

                        ExecuteResponse(Responses.SyntaxError, false);
                        errorCount++;
                    }
                    else
                        throw;
                }

            }
        }

        public Command ExecuteResponse(Response response, bool waitForCommand)
        {
            Command command = null;
            _writer.WriteLine(BattleShipProtocol.GetTcpResponse(response));

            if (waitForCommand)
                command = BattleShipProtocol.GetCommand(_reader.ReadLine());

            return command;
        }

        public Command ExecuteResponse(Response response, bool waitForCommand, params Commands[] validCommands)
        {
            Command command = null;
            _writer.WriteLine(BattleShipProtocol.GetTcpResponse(response));

            if (waitForCommand)
                command = GetCommand(validCommands);

            return command;
        }

        public Command ExecuteResponse(Responses resp, bool waitForCommand, string parameter = null)
        {
            Response response = new Response(resp, parameter);
            return ExecuteResponse(response, waitForCommand);
        }

        public Command ExecuteResponse(Responses resp, bool waitForCommand, string parameter, params Commands[] validCommands)
        {
            Response response = new Response(resp, parameter);
            return ExecuteResponse(response, waitForCommand, validCommands);
        }

        public void Dispose()
        {
            _reader?.Dispose();
            _writer?.Dispose();
            _networkStream?.Dispose();
            _client?.Close();
            _listener?.Stop();



        }
    }


}
