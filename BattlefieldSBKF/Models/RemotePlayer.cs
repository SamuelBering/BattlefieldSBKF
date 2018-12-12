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
        //StreamReader _reader;
        //StreamWriter _writer;
        WrappedStreamReader _reader;
        WrappedStreamWriter _writer;
        TcpClient _client;

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

        public void Connect(string host, int port, string localPlayerName)
        {
            try
            {
                _client = new TcpClient(host, port);
            }
            catch (SocketException ex)
            {
                throw new ClientNotCreatedException($"Kan ej starta förbindelse. Port {port} troligtvis upptagen");
            }
            var networkStream = _client.GetStream();
            _reader = new WrappedStreamReader(new StreamReader(networkStream, Encoding.UTF8), IsServer);
            _writer = new WrappedStreamWriter(new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true }, IsServer);

            Response response = GetResponse();

            if (response.Resp != Responses.Protocol)
                throw new UnExpectedResponseException($"Unexpected response {response.Resp}");

            Console.WriteLine(response.Resp + " " + response.Parameter);

            response = ExecuteCommand(Commands.Hello, true, localPlayerName);

            if (response.Resp != Responses.PlayerName)
                throw new UnExpectedResponseException($"Unexpected response {response.Resp}");

            Console.WriteLine(response.Resp + " " + response.Parameter);

            response = ExecuteCommand(Commands.Start, false, null);
        }

        public void Connect(int port, string localPlayerName)
        {
            TcpListener listener;

            try
            {
                listener = StartListen(port);
            }
            catch (SocketException ex)
            {
                throw new ListenerNotInitiatedException($"Kan ej starta förbindelse. Port {port} troligtvis upptagen");
            }

            _client = listener.AcceptTcpClient();
            var networkStream = _client.GetStream();
            //_reader = new StreamReader(networkStream, Encoding.UTF8);
            //_writer = new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true };
            _reader = new WrappedStreamReader(new StreamReader(networkStream, Encoding.UTF8), IsServer);
            _writer = new WrappedStreamWriter(new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true }, IsServer);

            Command command = ExecuteResponse(Responses.Protocol, true);

            if (command.Cmd != Commands.Hello)
                throw new UnExpectedCommandException($"Unexpected command: {command.Cmd}.");

            this.Name = string.Join(' ', command.Parameters);

            Console.WriteLine(command.Cmd + " " + string.Join(' ', command.Parameters));


            command = ExecuteResponse(Responses.PlayerName, true, localPlayerName);

            if (command.Cmd != Commands.Start)
                throw new UnExpectedCommandException($"Unexpected command: {command.Cmd}.");

            Console.WriteLine(command.Cmd);


        }

        public Response ExecuteCommand(Command command, bool waitForResponse)
        {
            var tcpCommand = BattleShipProtocol.GetTcpCommand(command);
            _writer.WriteLine(tcpCommand);

            if (waitForResponse)
            {
                //Response response = BattleShipProtocol.GetResponse(_reader.ReadLine());
                Response response = GetResponse();
                return response;
            }
            else
                return null;
        }

        public Response ExecuteCommand(Commands cmd, bool waitForResponse, params string[] parameters)
        {
            Command command = new Command(cmd, parameters);
            return ExecuteCommand(command, waitForResponse);
        }

        public Command GetCommand()
        {
            throw new NotImplementedException();
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


        public Response GetResponse()
        {
            return BattleShipProtocol.GetResponse(_reader.ReadLine());
        }

        public Command ExecuteResponse(Response response, bool waitForCommand)
        {
            Command command = null;
            _writer.WriteLine(BattleShipProtocol.GetTcpResponse(response));

            if (waitForCommand)
                command = BattleShipProtocol.GetCommand(_reader.ReadLine());

            return command;
        }

        public Command ExecuteResponse(Responses resp, bool waitForCommand, string parameter = null)
        {
            Response response = new Response(resp, parameter);
            return ExecuteResponse(response, waitForCommand);
        }

        public Command ExecuteResponse(Response response, Command initialCommand, bool waitForCommand)
        {
            throw new NotImplementedException();
        }
    }


}
