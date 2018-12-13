using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class GridBoard
    {
        private readonly IBattleShipProtocol _battleShipProtocol;

        protected char[] Grid { get; }
        public int GridSide { get; }
        protected char OceanSymbol { get; }

        public GridBoard(int gridSide, IBattleShipProtocol batProto)
        {
            Grid = new char[gridSide*gridSide];
            OceanSymbol = '~';
            for (int i = 0; i < Grid.Length; i++)
            {
                Grid[i] = OceanSymbol;
            }

            GridSide = gridSide;
            _battleShipProtocol = batProto;
        }

        public virtual void ShowBoard()
        {
            int k = 10;
            int d = 0;

            Console.WriteLine("-----------------------");
            Console.WriteLine("  1 2 3 4 5 6 7 8 9 10|");
            Console.WriteLine("-----------------------");

            for (int i = 0; i < 10; i++)
            {
                Console.Write($"{(char)(i+65)}|");
                for (int j = d; j < k; j++)
                {
                    Console.Write(Grid[j]);
                    Console.Write(" ");
                }

                Console.WriteLine("|");
                k += 10;
                d += 10;
            }
            Console.WriteLine("----------------------");
            Console.WriteLine();
        }

        protected int BoardCoordinateToIndex(string yCoord, string xCoord)
        {
            var index = GridSide * (_battleShipProtocol.YcordinateDict[yCoord] - 1)
                        + Int32.Parse(xCoord) - 1;
            return index;
        }


    }
}