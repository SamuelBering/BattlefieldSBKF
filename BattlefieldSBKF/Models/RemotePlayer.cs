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

            string answerCodeFromServer = _reader.ReadLine();


            AnswerCode answerCode = BattleShipProtocol.GetAnswerCode(answerCodeFromServer);

            if (answerCode.Name!="210")
                throw new UnExpectedAnswerCodeException($"Unexpected answer code: {answerCodeFromServer}. Expected answer code: {BattleShipProtocol.ServerConnected()}");

            Console.WriteLine(answerCodeFromServer);

            string helloTcpCommand = $"{BattleShipProtocol.HelloCmd()} {localPlayerName}";
            _writer.WriteLine(helloTcpCommand);

            //if (answerCode.)


        }

        public void Connect(int port)
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

            string serverConnectedAnswerCode = BattleShipProtocol.ServerConnected();

            _writer.WriteLine(serverConnectedAnswerCode);
            string tcpCommand = _reader.ReadLine();
            Command command = BattleShipProtocol.GetCommand(tcpCommand);
            if (command.Name != "HELLO")
                throw new UnExpectedCommandException($"Unexpected command: {command.Name}. Expected command: {BattleShipProtocol.HelloCmd()}");

            Console.WriteLine(tcpCommand);
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
