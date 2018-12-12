using BattlefieldSBKF.Models;
using System;
using System.Diagnostics;

namespace BattlefieldSBKF
{
    class Program
    {
        //static void GetUserInfo(out string host, out int port, out string playerName)
        //{
        //    string na;
        //    string ho;
        //    int po;
        //    bool isValidPortNumber;

        //    do
        //    {
        //        Console.Write("Ange player name: ");
        //        na = Console.ReadLine();
        //        Console.Write("Ange host: ");
        //        ho = Console.ReadLine();
        //        Console.Write("Ange port: ");
        //        isValidPortNumber = int.TryParse(Console.ReadLine(), out po);
        //        if (isValidPortNumber)
        //            break;
        //        Console.WriteLine("Port är ej i korrekt format");
        //    }
        //    while (true);


        //    host = ho;
        //    port = po;
        //    playerName = na;
        //}
        static bool AskUserToPlayAgain()
        {
            Console.Write($"Vill du spela igen? (j/n): ");
            while (true)
            {
                var answer = Console.ReadLine();
                if (answer.ToLower() == "j")
                    return false;
                else if (answer.ToLower() == "n")
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Ogiltigt val.");
                }
            }
        }

        static void Main(string[] args)
        {
            bool endGame = false;

            while (!endGame)
            {
                LocalPlayer localPlayer = new LocalPlayer();
                BattleShipProtocol battleShipProtocol = new BattleShipProtocol();
                RemotePlayer remotePlayer = new RemotePlayer(battleShipProtocol);

                using (BattleShipGameEngine battleShipGameEngine = new BattleShipGameEngine(localPlayer, remotePlayer))
                {
                    try
                    {
                        battleShipGameEngine.Run();
                    }
                    catch (Exception ex)
                    {

                        Debug.WriteLine($"Exception: {ex.ToString()}");
                        Console.WriteLine($"Ett fel inträffade: {ex.Message}");
                    }

                }

                endGame = AskUserToPlayAgain();

            }

        }
    }
}
