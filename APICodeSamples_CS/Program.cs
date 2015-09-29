using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICodeSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            GetUpData test = new GetUpData();
            
            test.GetApps();
            Console.ReadLine();

        }
    }
}
