﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class RemotePlayer : IPlayer
    {
        StreamReader _reader;
        StreamWriter _writer;

        public OceanGridBoard OceanGridBoard { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TargetGridBoard TargetGridBoard { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Command Connect(string host, int port)
        {
            throw new NotImplementedException();
        }

        public Command Connect(int port)
        {
            throw new NotImplementedException();
        }

        public string ExecuteCommand(Command command)
        {
            throw new NotImplementedException();
        }

        public Command GetCommand()
        {
            throw new NotImplementedException();
        }
    }
}
