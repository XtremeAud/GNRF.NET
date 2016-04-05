using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PcapDotNet.Packets;

namespace ExperimentCode
{
    public class InternalPacket
    {
        public string SrcName;
        public string DstName;
        public string Protocol;
        public Packet Packet;
        public string Actions;
    }
}
