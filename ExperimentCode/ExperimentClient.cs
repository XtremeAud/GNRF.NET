using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentCode
{
    public class ExperimentClient
    {
        public static void BlueTooth_Zigbee_Simulator()
        {
            Console.WriteLine(">> Choose the TX Interface please");
            // Retrieve the device list from the local machine
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;

            if (allDevices.Count == 0)
            {
                Console.WriteLine("No interfaces found! Make sure WinPcap is installed.");
                return;
            }

            // Print the list
            for (int i = 0; i != allDevices.Count; ++i)
            {
                LivePacketDevice device = allDevices[i];
                Console.Write((i + 1) + ". " + device.Name);
                if (device.Description != null)
                    Console.WriteLine(" (" + device.Description + ")");
                else
                    Console.WriteLine(" (No description available)");
            }

            GlobalSettings.InterfaceID_TX = 0;
            do
            {
                Console.WriteLine("Enter the interface number (1-" + allDevices.Count + "):");
                string deviceIndexString = Console.ReadLine();
                if (!int.TryParse(deviceIndexString, out GlobalSettings.InterfaceID_TX) ||
                    GlobalSettings.InterfaceID_TX < 1 || GlobalSettings.InterfaceID_TX > allDevices.Count)
                {
                    GlobalSettings.InterfaceID_TX = 0;
                }
            } while (GlobalSettings.InterfaceID_TX == 0);

            // Take the selected adapter
            PacketDevice selectedDevice = allDevices[GlobalSettings.InterfaceID_TX - 1];

            using (PacketCommunicator communicator =
                selectedDevice.Open(100, // name of the device                                                         
                PacketDeviceOpenAttributes.Promiscuous, // promiscuous mode
                1000)) // read timeout
            {

                while (true)
                {
                    communicator.SendPacket(BuildEthernetPacket());
                    Console.WriteLine("> SENDING TO->" + iPkt.Packet.Ethernet.Destination.ToString());
                }
            }
        }

        public static void EthReceiver()
        {
            throw new NotImplementedException();
        }

        public static void HTTPServer()
        {
            throw new NotImplementedException();
        }

        public static void CCNx()
        {
            throw new NotImplementedException();
        }

        private static Packet BuildEthernetPacket()
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
    }
}
