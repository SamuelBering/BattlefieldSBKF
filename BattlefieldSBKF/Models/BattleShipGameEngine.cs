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
        }

        public void Run()
        {
            try
            {
                if (_IsServer)
                    _remotePlayer.Connect(_port, _localPlayer.Name);
                else
                    _remotePlayer.Connect(_host, _port, _localPlayer.Name);


            }
            catch (ListenerNotInitiatedException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


    }
}
