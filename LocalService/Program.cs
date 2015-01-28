using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace LocalService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { 
                new ServiceLCO() 
            };
            ServiceBase.Run(ServicesToRun);
        }
        //https://msdn.microsoft.com/es-es/library/zt39148a(v=vs.110).aspx
        //http://noshonet.blogspot.mx/2012/03/creacion-de-servicios-de-windows-c-net.html
        //http://www.ajpdsoft.com/modules.php?name=News&file=article&sid=588
    }
}
