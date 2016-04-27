﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentCode
{
    public static class ExperimentSetting
    {
        //public static int PayloadLength = 10;

        public static string DstMACAdd = "00:50:56:32:1B:D0";
        //public static string SrcMACAdd = "00:50:56:32:1B:D0";
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
            else if (Input == "R")
            {
                ExperimentClient.EthReceiver();
            }
            else if (Input == "H")
            {
                ExperimentClient.HTTPServer();
            }
            else if (Input == "C")
            {
                ExperimentClient.CCNx();
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
