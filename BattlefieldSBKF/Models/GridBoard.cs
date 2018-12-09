using System;
using System.Runtime.CompilerServices;

namespace BattlefieldSBKF.Models
{
    public class GridBoard
    {
        protected char[] Grid { get; }
        public int GridSide { get; }
        protected char OceanSymbol { get; }

        public GridBoard(int gridSide)
        {
            Grid = new char[gridSide*gridSide];
            OceanSymbol = '~';
            for (int i = 0; i < Grid.Length; i++)
            {
                Grid[i] = '~';
            }

            GridSide = gridSide;
        }

        public void ShowBoard()
        {
            int k = 10;
            int d = 0;
            for (int i = 0; i < 10; i++)
            {

                for (int j = d; j < k; j++)
                {
                    Console.Write(Grid[j]);
                }

                Console.WriteLine();
                k += 10;
                d += 10;
            }
        }


    }
}