using System;
using System.Collections.Generic;
using System.Linq;

namespace BattlefieldSBKF.Models
{
    public enum Orientation
    {
        Vertical,
        Horizontal
    }

    public class OceanGridBoard: GridBoard
    {
        private readonly IList<Ship> _ships;
        
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

        private List<int> TempIndexArray { get; set; } = new List<int>();

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

        public string Fire(int gridIndex)
        {
            
            if (gridIndex < 0 || gridIndex > Grid.Length - 1)
            {
                throw new ArgumentException("Index out of range!");
            }

            if (!Char.IsLower(Grid[gridIndex]))
            {
                Grid[gridIndex] = Char.ToLower(Grid[gridIndex]);

                var ship = _ships.Single(s => s.Symbol == Char.ToUpper(Grid[gridIndex]) && s.IsDestroyed == false);
                ship.Length -= 1;
                
                if (ship.Length == 0)
                {
                    ship.IsDestroyed = true;
                    return $"Du sänkte mitt {ship.Name}";
                }

                return $"Träff. {ship.Name}";
            }
           
            return "Miss";
        }

        public bool IsAllShipsSunken()
        {
            return _ships.All(x => x.IsDestroyed);
        }

        


    }
}