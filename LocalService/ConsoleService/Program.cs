using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleService
{
    class Program
    {
        static void Main(string[] args)
        {
            string hourAppConfig = System.Configuration.ConfigurationManager.AppSettings["hour"];
            string hourNow = DateTime.Now.ToShortTimeString();
            Console.WriteLine(hourAppConfig);
            Console.WriteLine(hourNow);
            Console.ReadKey();
        }
    }
}
