using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class WrappedStreamReader
    {
        StreamReader _streamReader;
        bool _isServer;

        public WrappedStreamReader(StreamReader streamReader, bool isServer)
        {
            _streamReader = streamReader;
            _isServer = isServer;
        }

        public string ReadLine()
        {
            var data = _streamReader.ReadLine();

            if (_isServer)
            {
                Debug.WriteLine("Received from server: ");
                Debug.WriteLine(data);
            }
            else
            {
                Debug.WriteLine("Received from client: ");
                Debug.WriteLine(data);
            }
            return data;
        }
    }
}
