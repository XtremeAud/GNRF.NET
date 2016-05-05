using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentCode
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine(">> Experiment Mode:\nS: BlueTooth/Zigbee Sender Simulator R: EthReceiver C: CCNx Simulator N: Switch D: Downloading Test");
            string Input = Console.ReadLine();
            if (Input == "S")
            {
                ExperimentClient.BlueTooth_Zigbee_Simulator();
            }
            else if (Input == "R")
            {
                ExperimentClient.EthReceiver();
            }
            else if (Input == "C")
            {
                ExperimentClient.CCNxRequester();
            }
            else if (Input == "D")
            {
                ExperimentClient.HTTPDownloadTest();
            }
            else
            {
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
}
