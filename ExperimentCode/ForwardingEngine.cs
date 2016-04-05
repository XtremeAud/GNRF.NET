using PcapDotNet.Packets;
using System;
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
                        case "Eth":
                            {
                                EthForwarder(iPkt);
                                break;
                            }
                        case "TCP":
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

        private static void EthForwarder(InternalPacket iPkt)
        {

        }
    }
}
