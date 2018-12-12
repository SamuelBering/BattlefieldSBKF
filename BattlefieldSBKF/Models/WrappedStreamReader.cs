using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BattlefieldSBKF.Models
{
    public class WrappedStreamReader: IDisposable
    {
        StreamReader _streamReader;      
        bool _isServer;

        public StreamReader StreamReader
        {
            get
            {
                return _streamReader;
            }
        }

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

        public void Dispose()
        {
            _streamReader?.Dispose();
        }
    }
}
