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

    public static class HTTPGet
    {
        public static void HTTPGetFileAndForward(string URL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();

            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            // Take the selected adapter
            PacketDevice selectedDevice = allDevices[GlobalSettings.InterfaceID_RX - 1];

            using (PacketCommunicator communicator =
                selectedDevice.Open(100, // name of the device                                                         
                PacketDeviceOpenAttributes.Promiscuous, // promiscuous mode
                -1)) // read timeout
            {
                if (!response.ContentType.ToLower().StartsWith("text/"))
                {
                    byte[] PayloadBuffer = new byte[1024];
                    Stream inStream = response.GetResponseStream();
                    int Count = 1;
                    int l;
                    do
                    {
                        l = inStream.Read(PayloadBuffer, 0, PayloadBuffer.Length);
                        if (l > 0)
                        {
                            communicator.SendPacket(BuildEthernetPacket(PayloadBuffer, Count));
                        }
                    }
                    while (l > 0);
                    inStream.Close();
                }
            }
        }
        private static Packet BuildEthernetPacket(byte[] Payload, int Count)
        {
            PayloadLayer payloadLayer;
            payloadLayer =
            new PayloadLayer
            {
                Data = new Datagram(Payload),
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
