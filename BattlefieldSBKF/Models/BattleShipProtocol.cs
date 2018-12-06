using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class BattleShipProtocol : IBattleShipProtocol
    {
        public string HelloLocalPlayer()
        {
            return $"220";
        }

        public string LocalPlayerStart(string localPlayerName)
        {
            return $"222 Me, player {localPlayerName}, will start";
        }

        public string GetTcpAnswerCode(AnswerCode answerCode)
        {
            throw new NotImplementedException();
        }

        public string GetTcpCommand(Command command)
        {
            throw new NotImplementedException();
        }

        public AnswerCode GetAnswerCode(string tcpAnswerCode)
        {
            var substrings = tcpAnswerCode.Split(' ');
            bool success = false;

            if (tcpAnswerCode.ToLower() == ServerConnected().ToLower())
                success = true;
            else if (substrings[0] == HelloLocalPlayer())
                success = true;

            if (success)
                return new AnswerCode(substrings[0], substrings.Length > 1 ? 
                    string.Join(' ', substrings.Skip(1).Take(substrings.Length - 1).ToArray()) : null);
            else
                throw new CantCreateAnswerCodeException($"Can't create AnswerCode of input string {tcpAnswerCode}. Syntax error.");

        }

        public Command GetCommand(string tcpCommand)
        {
            var substrings = tcpCommand.Split(' ');
            bool success = false;

            if (substrings[0].ToLower() == HelloCmd().ToLower())
                success = true;
            else if (substrings[0].ToLower() == StartCmd().ToLower() && substrings.Length == 1)
                success = true;

            if (success)
                return new Command(substrings[0].ToUpper(), substrings.Length > 1 ? substrings.Skip(1).Take(substrings.Length - 1).ToArray() : null);
            else
                throw new CantCreateCommandException($"Can't create command of input string {tcpCommand}. Syntax error."); 
        }

        public string HelloCmd()
        {
            return "HELLO";
        }

        public string StartCmd()
        {
            return "START";
        }

        public string RemotePlayerStart(string remotePlayerName)
        {
            return $"221 You, player {remotePlayerName}, will start";
        }

        public string ServerConnected()
        {
            return "210 BATTLESHIP/1.0";
        }

        public string Start()
        {
            return "START\r\n";
        }
    }
}
