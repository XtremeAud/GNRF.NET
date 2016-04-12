using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExperimentCode
{
    public class ForwardingEngine
    {
        static Thread ForwardingEng;
        static public void StartForwardingEngine()
        {
            ForwardingEng = new Thread(tForwardingEng);
            ForwardingEng.Start();
        }

        private static void tForwardingEng()
        {
            while(true)
            {
                if(OutGoingPacketQueue.OutGoing.Count!=0)
                {
                    InternalPacket iPkt = OutGoingPacketQueue.OutGoing.Dequeue();
                    switch (iPkt.Protocol)
                    {
                        case InternalPacket.Protocols.TCP:
                            {
                                EthForwarder(iPkt);
                                break;
                            }
                        case InternalPacket.Protocols.UDP:
                            {
                                ;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }
        }

        private static Packet BuildEthernetPacket(InternalPacket iPkt)
        {
            byte[] Buffer_s = new byte[7];
            byte[] Buffer_d = new byte[7];
            byte[] Buffer_p = new byte[79];

            var bits = new BitArray(0);
            Buffer_s = Encoding.ASCII.GetBytes("AAAABBBB");
            Buffer_d = Encoding.ASCII.GetBytes("DDDDCCCC");
            Buffer_p = Encoding.ASCII.GetBytes("TEST_TEST_TEST_TEST");
            byte[] Buffer = new byte[127];
            Buffer_s.CopyTo(Buffer, 0);
            Buffer_d.CopyTo(Buffer, 8);
            Buffer_p.CopyTo(Buffer, 16);


            EthernetLayer ethernetLayer =
                new EthernetLayer
                {
                    Source = new MacAddress("A4:5E:60:D8:97:6F"),
                    Destination = new MacAddress("A4:5E:60:D8:97:5F"),
                    EtherType = EthernetType.IpV4,
                };

            PayloadLayer payloadLayer =
                new PayloadLayer
                {
                    Data = new Datagram(Buffer),
                };

            PacketBuilder builder = new PacketBuilder(ethernetLayer, payloadLayer);

            return builder.Build(DateTime.Now);
        }

        private static void EthForwarder(InternalPacket iPkt)
        {
            ;
        }
    }
}
