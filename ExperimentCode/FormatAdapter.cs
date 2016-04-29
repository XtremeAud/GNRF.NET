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

                if (!response.ContentType.ToLower().StartsWith("text/"))
            {
                //Value = SaveBinaryFile(response, FileName); 
                byte[] buffer = new byte[1024];
                //Stream outStream = System.IO.File.Create(FileName);
                Stream inStream = response.GetResponseStream();

                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0)
                        outStream.Write(buffer, 0, l);
                }
                while (l > 0);

                outStream.Close();
                inStream.Close();
            }
        } 
    }
}
