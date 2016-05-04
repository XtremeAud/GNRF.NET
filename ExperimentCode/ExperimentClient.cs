using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExperimentCode
{
    public static class ExperimentSetting
    {
        public static int PayloadLength = 0;

        public static string DstMACAdd = "00:50:56:2D:CC:20";
        public static string SrcMACAdd = "00:0C:29:B1:43:3B";
    }

    public class ExperimentClient
    {
        private static long Count = 0;

        private static byte[] Buffer;
        private static byte[] Name = new byte[10];
        private static PayloadLayer payloadLayer;
        private static EthernetLayer ethernetLayer;

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
                10)) // read timeout
            {
                //输入Payload长度
                Console.WriteLine("> Enter the length of the payload  >  10 500 1000 bytes...");
                ExperimentSetting.PayloadLength = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("> You set the length of the payload ->" + ExperimentSetting.PayloadLength.ToString());

                //选择实验模式，UDP或者Ethernet
                Console.WriteLine("> Ethernet or UDP? Tpye in E or U ...");
                string Input = Console.ReadLine();
                if (Input == "E")
                {
                    while (true)
                    {
                        communicator.SendPacket(BuildEthernetPacket());
                        //Console.WriteLine("> SENDING TO->" + ExperimentSetting.DstMACAdd);
                    }
                }
                else if (Input == "U")
                {
                    while (true)
                    {
                        communicator.SendPacket(BuildUdpPacket());
                        //Console.WriteLine("> SENDING TO->" + ExperimentSetting.DstMACAdd);
                    }
                }


            }
        }

        public static void EthReceiver()
        {

            //选择实验模式，UDP或者Ethernet
            Console.WriteLine("> Ethernet or UDP? Tpye in E or U ...");
            string Input = Console.ReadLine();
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

            int deviceIndex = 0;
            do
            {
                Console.WriteLine("Enter the interface number (1-" + allDevices.Count + "):");
                string deviceIndexString = Console.ReadLine();
                if (!int.TryParse(deviceIndexString, out deviceIndex) ||
                    deviceIndex < 1 || deviceIndex > allDevices.Count)
                {
                    deviceIndex = 0;
                }
            } while (deviceIndex == 0);

            // Take the selected adapter
            PacketDevice selectedDevice = allDevices[deviceIndex - 1];

            Thread tStat = new Thread(ReFresher);
            tStat.Start();

            // Open the device
            using (PacketCommunicator communicator =
                selectedDevice.Open(65536,                                  // portion of the packet to capture
                                                                            // 65536 guarantees that the whole packet will be captured on all the link layers
                                    PacketDeviceOpenAttributes.Promiscuous, // promiscuous mode
                                    10))                                  // read timeout
            {
                Console.WriteLine("Listening on " + selectedDevice.Description + "...");
                if (Input == "E")
                {
                    // start the capture
                    communicator.ReceivePackets(0, EthPacketHandler);
                }
                else if (Input == "U")
                {
                    // start the capture
                    communicator.ReceivePackets(0, EthPacketHandler);
                }
            }
        }

        private static void ReFresher()
        {
            while (true)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Pkt count:" + Count.ToString());
                Count = 0;
            }
        }

        private static void EthPacketHandler(Packet packet)
        {
            if (packet.Ethernet.Payload.Decode(System.Text.Encoding.ASCII).Contains("BL")) 
            {
                Count += 1;
            }
            //Console.WriteLine("From:" + packet.Ethernet.Source.ToString() + " length:" + packet.Length + " Msg: " + packet.Ethernet.Payload.Decode(System.Text.Encoding.ASCII));
        }

        private static void UDPPacketHandler(Packet packet)
        {
            //Console.WriteLine("From:" + packet.Ethernet.Source.ToString() + " length:" + packet.Length + " Msg: " + packet.Ethernet.Payload.Decode(System.Text.Encoding.ASCII));
            if (packet.IpV4.Payload.Decode(System.Text.Encoding.ASCII).Contains("BL")) 
            {
                Count += 1;
            }
        }

        private static Packet BuildEthernetPacket()
        {
            
            Name = Encoding.ASCII.GetBytes("BL#DOG#CAT");
            if (ExperimentSetting.PayloadLength == 10)
            {
                Buffer = new byte[20];
                Name.CopyTo(Buffer, 0);
            }
            else if (ExperimentSetting.PayloadLength == 500)
            {
                Buffer = new byte[510];
                Name.CopyTo(Buffer, 0);
            }
            else
            {
                Buffer = new byte[1010];
                Name.CopyTo(Buffer, 0);
            }
            payloadLayer =
            new PayloadLayer
            {
                Data = new Datagram(Buffer),
            };
            ethernetLayer =
            new EthernetLayer
            {
                Source = new MacAddress("FF:FF:FF:AA:AA:AA"),
                Destination = new MacAddress(ExperimentSetting.DstMACAdd),
                EtherType = EthernetType.IpV4,
            };
            PacketBuilder builder = new PacketBuilder(ethernetLayer, payloadLayer);
            return builder.Build(DateTime.Now);
        }

        private static Packet BuildUdpPacket()
        {
            EthernetLayer ethernetLayer =
                new EthernetLayer
                {
                    Source = new MacAddress("01:01:01:01:01:01"),
                    Destination = new MacAddress("02:02:02:02:02:02"),
                    EtherType = EthernetType.None, // Will be filled automatically.
                };

            IpV4Layer ipV4Layer =
                new IpV4Layer
                {
                    Source = new IpV4Address("192.168.1.2"),
                    CurrentDestination = new IpV4Address("192.168.1.1"),
                    Fragmentation = IpV4Fragmentation.None,
                    HeaderChecksum = null, // Will be filled automatically.
                    Identification = 123,
                    Options = IpV4Options.None,
                    Protocol = null, // Will be filled automatically.
                    Ttl = 100,
                    TypeOfService = 0,
                };

            UdpLayer udpLayer =
                new UdpLayer
                {
                    SourcePort = 4050,
                    DestinationPort = 25,
                    Checksum = null, // Will be filled automatically.
                    CalculateChecksumValue = true,
                };

            if (ExperimentSetting.PayloadLength == 10)
            {
                Buffer = new byte[20];
                Encoding.ASCII.GetBytes("BL#DOG#CAT").CopyTo(Buffer, 0);
            }
            else if (ExperimentSetting.PayloadLength == 500)
            {
                Buffer = new byte[510];
                Encoding.ASCII.GetBytes("BL#DOG#CAT").CopyTo(Buffer, 0);
            }
            else
            {
                Buffer = new byte[1010];
                Encoding.ASCII.GetBytes("BL#DOG#CAT").CopyTo(Buffer, 0);
            }
            PayloadLayer payloadLayer =
                new PayloadLayer
                {
                    Data = new Datagram(Buffer),
                };

            PacketBuilder builder = new PacketBuilder(ethernetLayer, ipV4Layer, udpLayer, payloadLayer);

            return builder.Build(DateTime.Now);
        }

        public static void CCNxRequester()
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
                10)) // read timeout
            {
                Console.WriteLine("> Request A File？ Y or N");
                string Input = Console.ReadLine();
                if (Input == "Y")
                {
                    while (true)
                    {
                        communicator.SendPacket(BuildEthernetPacket());
                        //Console.WriteLine("> SENDING TO->" + ExperimentSetting.DstMACAdd);
                    }
                }
                else if (Input == "N")
                {
                    return;
                }
            }
        }
    }
}
