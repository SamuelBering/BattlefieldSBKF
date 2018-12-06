using System;
using System.Runtime.CompilerServices;

namespace BattlefieldSBKF.Models
{
    public class GridBoard
    {
        protected char[] Grid { get; }
        public int GridSide { get; }

        public GridBoard(int gridSide)
        {
            Grid = new char[gridSide*gridSide];
            for (int i = 0; i < Grid.Length; i++)
            {
                Grid[i] = 'o';
            }

            GridSide = gridSide;
        }


    }
}