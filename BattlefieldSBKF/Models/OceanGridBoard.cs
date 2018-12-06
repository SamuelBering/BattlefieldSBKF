using System;
using System.Collections;
using System.Collections.Generic;
using BattlefieldSBKF.Models;

namespace BattlefieldSBKF.Models
{
    public enum Orientation
    {
        Vertical,
        Horizontal
    }

    public class OceanGridBoard: GridBoard
    {
        private readonly IEnumerable<Ship> _ships;
        
        public OceanGridBoard(int gridSide) : base(gridSide)
        {
            _ships = new List<Ship>()
            {
                new Carrier(),
                new Battleship(),
                new Destroyer(),
                new Submarine(),
                new PatrolBoat()
            };
            Initialize();
        }

        public List<int> TempIndexArray { get; set; } = new List<int>();

        private bool DrawShip(Ship ship, int gridIndex)
        {
            int y = gridIndex / GridSide;
            int x = gridIndex % GridSide;

            var orientation = GetOrientation();

            if (orientation == Orientation.Vertical)
            {
                if (y - ship.Length - 1 >= 0)
                {
                    return NewMethod(ship, ref gridIndex, orientation);
                }
                
            }

            if (orientation == Orientation.Horizontal)
            {
                if (x + ship.Length <= GridSide)
                {
                    return NewMethod(ship, ref gridIndex, orientation);

                }
            }

            return true;

        }

        private bool NewMethod(Ship ship, ref int gridIndex, Orientation orientation)
        {
            for (int i = 0; i < ship.Length; i++)
            {
                if (Grid[gridIndex] == 'o')
                {
                    TempIndexArray.Add(gridIndex);

                    if (orientation == Orientation.Horizontal)
                    {
                        gridIndex += 1;
                    }
                    else if (orientation == Orientation.Vertical)
                    {
                        gridIndex -= 10;
                    }
                    else throw new ArgumentException();
                }

            }

            if (TempIndexArray.Count == ship.Length)
            {
                foreach (var index in TempIndexArray)
                {
                    Grid[index] = ship.Symbol;
                }
                TempIndexArray.Clear();
                return false;
            }
            TempIndexArray.Clear();
            return true;
        }

        private void Initialize()
        {

            foreach (Ship ship in _ships)
            {
                bool result = true;
                while (result)
                {
                   result = DrawShip(ship, GetRandomIndex()); 
                }
                

            }

        }

        private Orientation GetOrientation()
        {
            Array values = Enum.GetValues(typeof(Orientation));
            Random rnd = new Random();

            return (Orientation)values.GetValue(rnd.Next(values.Length));
        }

        private int GetRandomIndex()
        {
            Random rnd = new Random();

            return rnd.Next(base.Grid.Length);
        }

        public void ShowBoard()
        {
            int k = 10;
            int d = 0;
            for (int i = 0; i < 10; i++)
            {

                for (int j=d; j < k; j++)
                {
                    Console.Write(base.Grid[j]);
                }

                Console.WriteLine();
                k += 10;
                d += 10;
            }
        }




    }
}