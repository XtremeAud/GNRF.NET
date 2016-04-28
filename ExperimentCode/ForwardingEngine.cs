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
            while (GlobalSettings.RXisOK != 1)
            {
                Thread.Sleep(1000);
            }
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
                -1)) // read timeout
            {

                while (true)
                {
                    if (OutGoingPacketQueue.OutGoing.Count > 0)
                    //if (InComingPacketQueue.InComing.Count > 0)
                    {
                        communicator.SendPacket(OutGoingPacketQueue.OutGoing.Dequeue().Packet);
                    }
                }
            }
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
