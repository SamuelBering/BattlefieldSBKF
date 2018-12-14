using BattlefieldSBKF.Models;
using System;
using System.Diagnostics;

namespace BattlefieldSBKF
{
    class Program 
    {
        
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
