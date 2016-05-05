using PcapDotNet.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentCode
{
    public class Interpreter
    {
        public static void ProcessNDNInterestPkt(string URL)
        {
            HTTPGet.HTTPGetFileAndForward(URL);
        }
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
                    byte[] PayloadBuffer = new byte[1000];
                    DateTime st = DateTime.Now; 
                    Stream inStream = response.GetResponseStream();
                    int Count = 1;
                    int l;
                    do
                    {
                        l = inStream.Read(PayloadBuffer, 0, PayloadBuffer.Length);
                        if (l > 0)
                        {

                            communicator.SendPacket(NDNPackets.EthernetNDNContentPacket(PayloadBuffer, Count, "1234", "11"));
                            Console.WriteLine("Sent Pakcets: "+ Count);
                        }
                        Count += 1;
                    }
                    while (l > 0);
                    communicator.SendPacket(NDNPackets.EthernetNDNContentPacket(new byte[1], Count, "1234", "88"));
                    Console.WriteLine("> Sent Pakcets: " + Count);
                    inStream.Close();
                    DateTime en = DateTime.Now;
                    TimeSpan ts = en - st;
                    Console.WriteLine(">>>>>>>>>>>>>>> Time: " + ts.TotalSeconds);
                }
            }
        }
    }


}
