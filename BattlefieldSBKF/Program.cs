using BattlefieldSBKF.Models;
using System;

namespace BattlefieldSBKF
{
    class Program
    {
        static void GetUserInfo(out string host, out int port, out string playerName)
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
                if (isValidPortNumber)
                    break;
                Console.WriteLine("Port är ej i korrekt format");
            }
            while (true);


            host = ho;
            port = po;
            playerName = na;
        }

        static void Main(string[] args)
        {


            GetUserInfo(out string host, out int port, out string playerName);

            LocalPlayer localPlayer = new LocalPlayer(playerName);

            BattleShipProtocol battleShipProtocol = new BattleShipProtocol();

            RemotePlayer remotePlayer = new RemotePlayer(battleShipProtocol);


            BattleShipGameEngine battleShipGameEngine = new BattleShipGameEngine(localPlayer, remotePlayer, host, port);


            battleShipGameEngine.Run();

            Console.ReadKey();
        }
    }
}
