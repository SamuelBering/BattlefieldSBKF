using System;
using System.Collections.Generic;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public interface IBattleShipProtocol
    {

        string HelloCmd();

        string StartCmd();

        Command GetCommand(string tcpCommand);
        string GetTcpCommand(Command command);

        AnswerCode GetAnswerCode(string tcpAnswerCode);
        string GetTcpAnswerCode(AnswerCode answerCode);

        string ServerConnected();

        string HelloLocalPlayer();

        string LocalPlayerStart(string localPlayerName);

        string RemotePlayerStart(string remotePlayerName);

        string Start();



    }
}
