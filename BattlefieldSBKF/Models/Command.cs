using System.Collections.Generic;

namespace BattlefieldSBKF.Models
{
    public class Command
    {
        public string Name { get; set; }
        public string[] Parameters { get; set; }

        public Command(string name, params string[] parameters)
        {
            Name = name;
            Parameters = parameters;
        }
    }
}