using Mountain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacketCaptureInfo
{
    class Client
    {
        public ConnectionState State { get; set; }
        //public bool Disconnected { get; set; }
        public int CompressionThreshold { get; set; }
        public bool UseCompression => CompressionThreshold > 0;

        public Client()
        {
            State = ConnectionState.Status;
            //Disconnected = true;
            CompressionThreshold = 0;
        }
    }
}
