using PcapDotNet.Core;
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
                    if (OutGoingPacketQueue.OutGoing.Count != 0)
                    {
                        InternalPacket iPkt = OutGoingPacketQueue.OutGoing.Dequeue();
                        //switch (iPkt.Protocol)
                        //{
                        //    case InternalPacket.Protocols.TCP:
                        //        {
                        //            communicator.SendPacket(OutGoingPacketQueue.OutGoing.Dequeue());
                        //            break;
                        //        }
                        //    case InternalPacket.Protocols.UDP:
                        //        {
                        //            EthForwarderLayer_3(iPkt);
                        //            break;
                        //        }
                        //    default:
                        //        {
                        //            EthForwarder(iPkt);
                        //            break;
                        //        }
                        //}
                        communicator.SendPacket(iPkt.Packet);
                        Console.WriteLine("> SENDING TO->" + iPkt.Packet.Ethernet.Destination.ToString());
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

        private static void EthForwarderLayer_3(InternalPacket iPkt)
        {
            ;
        }
    }
}
