using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class RemotePlayer : IPlayer
    {
        StreamReader _reader;
        StreamWriter _writer;
        TcpClient _client;

        public OceanGridBoard OceanGridBoard { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TargetGridBoard TargetGridBoard { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IBattleShipProtocol BattleShipProtocol { get; set; }
        public string Name { get; set; }

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
            _reader = new StreamReader(networkStream, Encoding.UTF8);
            _writer = new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true };

            Response response = BattleShipProtocol.GetResponse(_reader.ReadLine());

            if (response.Resp != Responses.Protocol)
                throw new UnExpectedAnswerCodeException($"Unexpected response {response.Resp}");

            Console.WriteLine(response.Resp + " " + response.Parameter);

            Command command = new Command(Commands.Hello, localPlayerName);
            
            _writer.WriteLine(BattleShipProtocol.GetTcpCommand(command));

             response = BattleShipProtocol.GetResponse(_reader.ReadLine());

            Console.WriteLine(response.Resp + " " + response.Parameter);

            //if (answerCode.)


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
            _reader = new StreamReader(networkStream, Encoding.UTF8);
            _writer = new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true };

            Response response = new Response(Responses.Protocol, BattleShipProtocol.ProtocolName);

            //string serverConnectedAnswerCode = BattleShipProtocol.ServerConnected();

            _writer.WriteLine(BattleShipProtocol.GetTcpResponse(response));

            Command command = BattleShipProtocol.GetCommand(_reader.ReadLine());

          
            if (command.Cmd != Commands.Hello)
                throw new UnExpectedCommandException($"Unexpected command: {command.Cmd}.");

            Console.WriteLine(command.Cmd+" "+string.Join(' ', command.Parameters));

            response = new Response(Responses.PlayerName, localPlayerName);

            _writer.WriteLine(BattleShipProtocol.GetTcpResponse(response));

        }

        public string ExecuteCommand(Command command)
        {
            throw new NotImplementedException();
        }

        public Command GetCommand()
        {
            throw new NotImplementedException();
        }
    }
}
