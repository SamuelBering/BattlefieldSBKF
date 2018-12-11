using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class BattleShipGameEngine
    {
        IPlayer _localPlayer;
        IPlayer _remotePlayer;
        bool _IsServer = false;
        string _host;
        int _port;

        public BattleShipGameEngine(IPlayer localPlayer, IPlayer remotePlayer, string host, int port)
        {
            _localPlayer = localPlayer;
            _remotePlayer = remotePlayer;
            _host = host;
            _port = port;

            if (string.IsNullOrEmpty(_host))
                _IsServer = true;

            _localPlayer.IsServer = _IsServer;
            _remotePlayer.IsServer = !_IsServer;
        }


        void RunAsServer()
        {
            bool endGame = false;
        }

        void RunAsClient()
        {
            bool endGame = false;
        }

        public void Run()
        {
            try
            {
                bool endGame = false;

                bool localPlayerStart = true;

                if (_IsServer)
                {
                    _remotePlayer.Connect(_port, _localPlayer.Name);
                }
                else
                {
                    _remotePlayer.Connect(_host, _port, _localPlayer.Name);
                    //hämta svar om vem som startar från serven
                    var response = _remotePlayer.GetResponse();
                    if (response.Resp == Responses.HostStarts)
                        localPlayerStart = false;
                }




                /*
                (Loop)
                om localplayerstart==true
                     hämta kommand eller response från localplayer
                     
                     om kommando                        
                        utför kommando på remoteplayer
                     annars 
                        utför response på remoteplayer

                     hämta svar från remoteplayer
                     utför svar på localplayer

                hämta kommando eller response från remoteplayer
                 
                om kommando
                     utför kommando på localplayer
                  annars
                     om response=connectionclosed
                        stäng programmet

                  localplayerstart=true
                 
                 
                 */

                while (!endGame)
                {
                    Command command;
                    Response response;

                    if (localPlayerStart)
                    {
                        _localPlayer.GetCommandOrResponse(out command, out response);

                        if (command != null)
                        {
                           
                            if (command.Cmd == Commands.Quit)
                            {
                                //jag är här!! fortsätt imorg...(se ovan)
                                response = _remotePlayer.ExecuteCommand(command, true);
                                if (response.Resp == Responses.ConnectionClosed)
                                {
                                    endGame = true;
                                    continue;
                                }
                                else
                                    throw new UnExpectedResponseException($"Expected response: {Responses.ConnectionClosed} but instead got: {response.Resp}.");
                            }
                            else
                            {
                                response = _remotePlayer.ExecuteCommand(command, true);
                            }
                        }
                        else
                        {
                            if (response.Resp == Responses.ConnectionClosed)
                            {
                                _remotePlayer.ExecuteResponse(response, false);
                                endGame = true;
                                continue;
                            }
                            else
                                throw new UnExpectedResponseException($"Expected response: {Responses.ConnectionClosed} but instead got: {response.Resp}.");
                        }

                        _localPlayer.ExecuteResponse(response, false);

                        _remotePlayer.GetCommandOrResponse(out command, out response);
                        if (command != null)
                        {
                            if (command.Cmd == Commands.Quit)
                            {
                            }
                            else
                            {

                            }
                                response = _localPlayer.ExecuteCommand(command, false);
                        }
                        else
                        {
                            Console.WriteLine($"Fel: Programmet stängs: {response.ToString()}");
                            endGame = true;
                            continue;
                        }



                    }
                    localPlayerStart = true;
                }


            }

            catch (ListenerNotInitiatedException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


    }
}
