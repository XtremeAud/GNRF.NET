using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentCode
{
    public static class ExperimentSetting
    {
        public static int PayloadLength = 10;
        
        //Switch_0 是接收者
        public static string DstMACAdd = 
    }
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine(">> Experiment Mode:\nS: BlueTooth/Zigbee Sender Simulator R: EthReceiver H: HTTP Server C: CCNx Simulator N: Switch");
            string Input = Console.ReadLine();
            if (Input == "S")
            {
                ExperimentClient.BlueTooth_Zigbee_Simulator();
            }
            if (Input == "R")
            {

            }

            EthInterface.StartingRX();
            Console.WriteLine("GMN Switch is starting...");
            Console.WriteLine(">> RX is running...");
            ResolutionModule.StartResolutin();
            Console.WriteLine(">> Resolution module is running...");
            ForwardingEngine.StartForwardingEngine();
            Console.WriteLine(">> TX is running...");
        }
    }
}
