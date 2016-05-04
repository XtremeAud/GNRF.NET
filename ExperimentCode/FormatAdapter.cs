using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentCode
{
    class FormatAdapter
    {
        
    }

    public static class NDNPackets
    {
        public static Packet EthernetNDNContentPacket(byte[] Payload, int Count, string FragmentStreamID, string Flag)
        {
            byte[] buffer = new byte[1024];
            
            //Header
            byte[] Version = Encoding.ASCII.GetBytes("1");
            byte[] Type = Encoding.ASCII.GetBytes("1");
            byte[] _FragmentStreamID = new byte[4];
            _FragmentStreamID = Encoding.ASCII.GetBytes(FragmentStreamID);
            
            //Put this field at [16-17] to indicate the ending signal
            byte[] _Flag = Encoding.ASCII.GetBytes(Flag);

            Version.CopyTo(buffer, 0);
            Type.CopyTo(buffer, 1);
            _FragmentStreamID.CopyTo(buffer, 12);
            _Flag.CopyTo(buffer, 16);
            Payload.CopyTo(buffer, 24);

            PayloadLayer payloadLayer =
            new PayloadLayer
            {
                Data = new Datagram(buffer),
            };
            EthernetLayer ethernetLayer =
            new EthernetLayer
            {
                Source = new MacAddress(ExperimentSetting.SrcMACAdd),
                Destination = new MacAddress(ExperimentSetting.DstMACAdd),
                EtherType = EthernetType.IpV4,
            };
            PacketBuilder builder = new PacketBuilder(ethernetLayer, payloadLayer);
            return builder.Build(DateTime.Now);
        }

        public static Packet EthernetInterestPacket(string ContentName)
        {
            byte[] buffer = new byte[1024];

            //Header
            byte[] Version = Encoding.ASCII.GetBytes("1");
            byte[] Type = Encoding.ASCII.GetBytes("1");

            Version.CopyTo(buffer, 0);
            Type.CopyTo(buffer, 1);

            PayloadLayer payloadLayer =
            new PayloadLayer
            {
                Data = new Datagram(buffer),
            };
            EthernetLayer ethernetLayer =
            new EthernetLayer
            {
                Source = new MacAddress(ExperimentSetting.SrcMACAdd),
                Destination = new MacAddress(ExperimentSetting.DstMACAdd),
                EtherType = EthernetType.IpV4,
            };
            PacketBuilder builder = new PacketBuilder(ethernetLayer, payloadLayer);
            return builder.Build(DateTime.Now);
        }
    }
}
