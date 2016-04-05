using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PcapDotNet.Core;
using PcapDotNet.Packets;

namespace ExperimentCode
{
    public static class InComingPacketQueue
    {
        public static Queue<Packet> InComing = new Queue<Packet>();
    }

    public static class OutGoingPacketQueue
    {
        public static Queue<InternalPacket> OutGoing = new Queue<InternalPacket>();
    }
}
