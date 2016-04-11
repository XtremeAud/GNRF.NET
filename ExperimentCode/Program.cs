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
            Console.WriteLine("GMN Switch is starting...");
            EthInterface.StartingRX();
            Console.WriteLine(">> RX is running...");
            ResolutionModule.StartResolutin();
            Console.WriteLine(">> Resolution module is running...");
            ForwardingEngine.StartForwardingEngine();
            Console.WriteLine(">> TX is running...");
        }
    }
}
