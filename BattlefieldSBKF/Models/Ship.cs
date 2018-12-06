using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public abstract class Ship
    {
        public Ship(char symbol, string name, bool isDestroyed, int length)
        {
            Symbol = symbol;
            Name = name;
            IsDestroyed = isDestroyed;
            Length = length;
        }

        public char Symbol { get; set; }
        public string Name { get; set; }
        public bool IsDestroyed { get; set; }
        public int Length { get; set; }
    }
}
