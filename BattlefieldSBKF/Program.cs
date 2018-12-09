using System;
using BattlefieldSBKF.Models;

namespace BattlefieldSBKF
{
    class Program
    {
        static void Main(string[] args)
        {
            var oceanBoard = new OceanGridBoard(10);
            oceanBoard.ShowBoard();

            Console.ReadKey();

        }
    }
}
