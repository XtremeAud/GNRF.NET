using PcapDotNet.Core;
using PcapDotNet.Packets;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ExperimentCode
{

    public static class GlobalSettings
    {
        public static int InterfaceID_TX = 0;
        public static int InterfaceID_RX = 0;
        public static int RXisOK = 0;
    }

    //接受Ethernet报文并加入到Incoming队列中
    class EthInterface
    {

        public static void StartingRX()
        {
            Thread RX_Tread = new Thread(RX_Callback);
            RX_Tread.Start();
        }

        private static void RX_Callback()
        {
            // Retrieve the device list from the local machine
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;

            if (allDevices.Count == 0)
            {
                Console.WriteLine("No interfaces found! Make sure WinPcap is installed.");
                return;
            }

            // Print the list
            Console.WriteLine(">> Choose the RX Interface please");
            for (int i = 0; i != allDevices.Count; ++i)
            {
                LivePacketDevice device = allDevices[i];
                Console.Write((i + 1) + ". " + device.Name);
                if (device.Description != null)
                    Console.WriteLine(" (" + device.Description + ")");
                else
                    Console.WriteLine(" (No description available)");
            }

            GlobalSettings.InterfaceID_RX = 0;
            do
            {
                Console.WriteLine("Enter the interface number (1-" + allDevices.Count + "):");
                string deviceIndexString = Console.ReadLine();
                if (!int.TryParse(deviceIndexString, out GlobalSettings.InterfaceID_RX) ||
                    GlobalSettings.InterfaceID_RX < 1 || GlobalSettings.InterfaceID_RX > allDevices.Count)
                {
                    GlobalSettings.InterfaceID_RX = 0;
                }
            } while (GlobalSettings.InterfaceID_RX == 0);

            // Take the selected adapter
            //PacketDevice selectedDevice = allDevices[deviceIndex - 1];
            PacketDevice selectedDevice = allDevices[GlobalSettings.InterfaceID_RX - 1];
            GlobalSettings.RXisOK = 1;

            // Open the device
            using (PacketCommunicator communicator =
                selectedDevice.Open(65536,                                  // portion of the packet to capture
                                                                            // 65536 guarantees that the whole packet will be captured on all the link layers
                                    PacketDeviceOpenAttributes.Promiscuous, // promiscuous mode
                                    -1))                                  // read timeout
            {
                Console.WriteLine(">> Listening on " + selectedDevice.Description + "...");

                // start the capture
                communicator.ReceivePackets(0, PacketHandler);
            }
        }

        private static void PacketHandler(Packet packet)
        {
            //Console.WriteLine("From:" + packet.Ethernet.Source.ToString() + " length:" + packet.Length + " Msg: " + packet.Ethernet.Payload.Decode(System.Text.Encoding.UTF8));
            //Console.WriteLine(packet.Ethernet.Payload.ToHexadecimalString());

            InComingPacketQueue.InComing.Enqueue(packet);
            //Console.WriteLine(packet.Length);
        }
    }
}
