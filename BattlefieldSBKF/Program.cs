using System;
using BattlefieldSBKF.Models;

namespace BattlefieldSBKF
{
    class Program
    {
        static void Main(string[] args)
        {
            var oceanBoard = new OceanGridBoard(10);
            var targetBoard = new TargetGridBoard(10);
            
           
            int shotIndex;
            bool IsGameOn = true;
            string resultMessage;

            do
            {
                oceanBoard.ShowBoard();
                Console.WriteLine();
                targetBoard.ShowBoard();
                shotIndex = Convert.ToInt32(Console.ReadLine());
                resultMessage = oceanBoard.Fire(shotIndex);
                Console.WriteLine(resultMessage);
                var test = resultMessage.Contains("Träff");
                targetBoard.MarkShot(shotIndex, resultMessage.Contains("Träff"));

                if (oceanBoard.IsAllShipsSunken())
                {
                    IsGameOn = false;
                }
            } while (IsGameOn);

            Console.WriteLine("GAME OVER!!");
            Console.ReadKey();

        }
    }
}
