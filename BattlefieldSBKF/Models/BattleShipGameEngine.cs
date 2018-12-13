using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class BattleShipGameEngine : IDisposable
    {
        IPlayer _localPlayer;
        IPlayer _remotePlayer;
        bool _IsServer = false;
        string _host;
        int _port;
        

        public BattleShipGameEngine(IPlayer localPlayer, IPlayer remotePlayer)
        {
            _localPlayer = localPlayer;
            _remotePlayer = remotePlayer;
        }

        void Init(string host, int port)
        {
            _host = host;
            _port = port;

            if (string.IsNullOrEmpty(_host))
                _IsServer = true;

            _localPlayer.IsServer = _IsServer;
            _remotePlayer.IsServer = !_IsServer;
        }

        void ExecuteLocalPlayerTurnAsServer(ref bool endGame)
        {
            _localPlayer.GetCommandOrResponse(out Command command, out Response response);

            if (command != null)
            {
                if (command.Cmd == Commands.Fire)
                {
                    response = _remotePlayer.ExecuteCommand(command, true);
                    if ((int)response.Resp >= 4 && (int)response.Resp <= 15)
                    {
                        _localPlayer.ExecuteResponse(response, false);
                        return;
                    }
                    else
                        throw new UnExpectedResponseException($"Expected miss-, hit-, sunk- or win-response but instead got: {response.Resp}.");
                }
                else
                    throw new UnExpectedCommandException($"Expected command: {Commands.Fire} but instead got: {command.Cmd}");
            }
            else
            {
                if (response.Resp == Responses.ConnectionClosed)
                {
                    _remotePlayer.ExecuteResponse(response, false);
                    endGame = true;
                    return;
                }
                else
                    throw new UnExpectedResponseException($"Expected response: {Responses.ConnectionClosed} but instead got: {response.Resp}.");

            }
        }

        void ExecuteLocalPlayerTurnAsClient(ref bool endGame)
        {
            _localPlayer.GetCommandOrResponse(out Command command, out Response response);

            if (command.Cmd == Commands.Fire)
            {
                response = _remotePlayer.ExecuteCommand(command, true);

                if ((int)response.Resp >= 4 && (int)response.Resp <= 15)
                {
                    _localPlayer.ExecuteResponse(response, false);
                    return;
                }
                else
                    throw new UnExpectedResponseException($"Expected miss-, hit-, sunk- or win-response but instead got: {response.Resp}.");


            }
            else if (command.Cmd == Commands.Quit)
            {
                response = _remotePlayer.ExecuteCommand(command, true);

                if (response.Resp == Responses.ConnectionClosed)
                {
                    endGame = true;
                    return;
                }
                else
                    throw new UnExpectedResponseException($"Expected response: {Responses.ConnectionClosed} but instead got: {response.Resp}.");

            }
            else
                throw new UnExpectedCommandException($"Expected command: {Commands.Fire} or {Commands.Quit} but instead got: {command.Cmd}");

        }

        void ExecuteRemotePlayerTurnAsServer(ref bool endGame)
        {
            _remotePlayer.GetCommandOrResponse(out Command command, out Response response);

            if (command.Cmd == Commands.Fire)
            {
                response = _localPlayer.ExecuteCommand(command, true);
                _remotePlayer.ExecuteResponse(response, false);
                return;
            }
            else if (command.Cmd == Commands.Quit)
            {
                _remotePlayer.ExecuteResponse(Responses.ConnectionClosed, false, null);
                endGame = true;
                return;
            }

        }
        void ExecuteRemotePlayerTurnAsClient(ref bool endGame)
        {
            _remotePlayer.GetCommandOrResponse(out Command command, out Response response);

            if (command != null)
            {
                if (command.Cmd == Commands.Fire)
                {
                    response = _localPlayer.ExecuteCommand(command, true);
                    _remotePlayer.ExecuteResponse(response, false);
                    return;
                }
                else
                    throw new UnExpectedCommandException($"Expected command: {Commands.Fire} but instead got: {command.Cmd}");

            }
            else
            {
                if (response.Resp == Responses.ConnectionClosed)
                {
                    endGame = true;
                    return;
                }
                else
                    throw new UnExpectedResponseException($"Expected response: {Responses.ConnectionClosed} but instead got: {response.Resp}.");

            }

        }

        void RunAsServer(bool localPlayerStart)
        {
            bool endGame = false;

            while (!endGame)
            {
                if (localPlayerStart)
                    ExecuteLocalPlayerTurnAsServer(ref endGame);

                if (endGame)
                    continue;

                ExecuteRemotePlayerTurnAsServer(ref endGame);

                if (endGame)
                    continue;

                localPlayerStart = true;
            }

        }

        void RunAsClient(bool localPlayerStart)
        {
            bool endGame = false;

            while (!endGame)
            {
                if (localPlayerStart)
                    ExecuteLocalPlayerTurnAsClient(ref endGame);

                if (endGame)
                    continue;

                ExecuteRemotePlayerTurnAsClient(ref endGame);

                if (endGame)
                    continue;

                localPlayerStart = true;
            }

        }

        void RunGame()
        {
            bool localPlayerStart = true;

            if (_IsServer)
            {
                if (!_remotePlayer.Connect(_port, _localPlayer.Name))
                    return;
                Random rnd = new Random();
                localPlayerStart = rnd.Next(0, 2) == 1 ? true : false;
                //localPlayerStart = true;
                Responses resp = localPlayerStart ? Responses.HostStarts : Responses.ClientStarts;
                _remotePlayer.ExecuteResponse(resp, false, null);
                RunAsServer(localPlayerStart);
            }
            else
            {
                if (!_remotePlayer.Connect(_host, _port, _localPlayer.Name))
                    return;               
                var response = _remotePlayer.GetResponse(Responses.HostStarts,Responses.ClientStarts);
                if (response.Resp == Responses.HostStarts)
                    localPlayerStart = false;
                RunAsClient(localPlayerStart);
            }
        }


        void GetUserInfo(out string host, out int port, out string playerName)
        {
            string na;
            string ho;
            int po;
            bool isValidPortNumber;

            do
            {
                Console.Write("Ange player name: ");
                na = Console.ReadLine();
                Console.Write("Ange host: ");
                ho = Console.ReadLine();
                Console.Write("Ange port: ");
                isValidPortNumber = int.TryParse(Console.ReadLine(), out po);
                if (isValidPortNumber && na.Length > 0)
                    break;
                Console.WriteLine("Port och eller spelarens namn är ej korrekt angiven.");
            }
            while (true);


            host = ho;
            port = po;
            playerName = na;
        }

        public void Run()
        {
            Console.Clear();

            GetUserInfo(out string host, out int port, out string playerName);
            _localPlayer.Name = playerName;
            Init(host, port);

            RunGame();
        }

        public void Dispose()
        {
            _remotePlayer.Dispose();
        }
    }
}
