using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class RemotePlayer : IPlayer
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

            Response response = GetResponse(Responses.Protocol);
            if (response.Resp == Responses.ConnectionClosed)
                return false;
            //if (response.Resp != Responses.Protocol)
            //    throw new UnExpectedResponseException($"Unexpected response {response.Resp}. Expected response: {Responses.Protocol}");

            response = ExecuteCommand(Commands.Hello, waitForResponse: true,
                validResponses: new Responses[] { Responses.PlayerName }, parameters: localPlayerName);
            if (response.Resp == Responses.ConnectionClosed)
                return false;
            //if (response.Resp != Responses.PlayerName)
            //    throw new UnExpectedResponseException($"Unexpected response {response.Resp}");

            this.Name = response.Parameter;
            Console.WriteLine($"Du spelar mot: {this.Name}\tIpaddress: {_client.Client.RemoteEndPoint}");
            response = ExecuteCommand(Commands.Start, false, null,null);
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


            Command command = ExecuteResponse(Responses.Protocol, waitForCommand: true, parameter: null, validCommands: Commands.Hello);
            if (command.Cmd == Commands.Quit)
                return false;

            this.Name = string.Join(' ', command.Parameters);

            Console.WriteLine($"Du spelar mot: {this.Name}\tIpaddress: {_client.Client.RemoteEndPoint}");

            command = ExecuteResponse(Responses.PlayerName, waitForCommand: true, parameter: localPlayerName, validCommands: Commands.Start);
            if (command.Cmd == Commands.Quit)
                return false;
            //command = ExecuteResponse(Responses.PlayerName, true, localPlayerName);

            //if (command.Cmd != Commands.Start)
            //    throw new UnExpectedCommandException($"Unexpected command: {command.Cmd}.");
            return true;
        }

        public Response ExecuteCommand(Command command, bool waitForResponse, params Responses[] validResponses)
        {
            Response response = null;

            var tcpCommand = BattleShipProtocol.GetTcpCommand(command);
            _writer.WriteLine(tcpCommand);

            if (waitForResponse)
                response = GetResponse(validResponses);

            return response;

            //if (waitForResponse)
            //{

            //    Response response = GetResponse();
            //    return response;
            //}
            //else
            //    return null;
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
                    throw new ExceededErrorCodesLimitSent($"Number of error codes sent exceeded maxium(3).");

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


        public Response GetResponse(params Responses[] validResponses)
        {
            Command command = null;
            Response response = null;

            int errorCount = 0;
            while (true)
            {
                if (errorCount > 3)
                    throw new ExceededErrorCodesLimitSent($"Number of error codes sent exceeded maxium(3).");

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

            //if (waitForCommand)
            //{
            //    int errorCount = 0;
            //    while (true)
            //    {
            //        if (errorCount > 3)
            //            throw new ExceededErrorCodesLimitSent($"Number of error codes sent exceeded maxium(3).");

            //        try
            //        {
            //            bool isValidCommand = false;

            //            command = BattleShipProtocol.GetCommand(_reader.ReadLine());
            //            foreach (Commands validCommand in validCommands)
            //            {
            //                if (command.Cmd == validCommand)
            //                    isValidCommand = true;
            //            }

            //            if (isValidCommand)
            //                return command;
            //            else
            //            {
            //                ExecuteResponse(Responses.SequenceError, false);
            //                errorCount++;
            //            }
            //        }
            //        catch (CantCreateCommandException ex)
            //        {
            //            ExecuteResponse(Responses.SyntaxError, false);
            //            errorCount++;
            //        }
            //    }
            //}
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

        public Command ExecuteResponse(Response response, Command initialCommand, bool waitForCommand)
        {
            throw new NotImplementedException();
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
