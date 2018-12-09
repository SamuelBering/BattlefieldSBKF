using System.Collections.Generic;

namespace BattlefieldSBKF.Models
{
    public class Command
    {
        public Commands Cmd { get; set; }
        public string[] Parameters { get; set; }

        public Command(Commands cmd, params string[] parameters)
        {
            Cmd = cmd;
            Parameters = parameters;
        }
    }
}