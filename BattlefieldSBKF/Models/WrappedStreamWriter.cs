using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class WrappedStreamWriter
    {
        StreamWriter _streamWriter;
        bool _isServer;

        public WrappedStreamWriter(StreamWriter streamWriter, bool isServer)
        {
            _streamWriter = streamWriter;
            _isServer = isServer;
        }

        public void WriteLine(string data)
        {
            _streamWriter.WriteLine(data);

            //if (_isServer)
            //{
            //    Debug.WriteLine("Sent by server: ");
            //    Debug.WriteLine(data);
            //}
            //else
            //{
            //    Debug.WriteLine("Sent by client: ");
            //    Debug.WriteLine(data);
            //}
           
        }
    }
}
