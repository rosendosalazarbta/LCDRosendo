using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;

namespace WebService
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://localhost.org")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld(){
            return "Hello World";
        }

        [WebMethod]
        public void UpdateLCO(){
            Operation operation = new Operation();
            string mapPath = System.Web.Hosting.HostingEnvironment.MapPath("~" + @"\LogInfo\LogStart.txt");
            
            StreamWriter sw = new StreamWriter(mapPath);
            sw.WriteLine("En Ejecución...");
            sw.Close();
            
            string log = operation.Start();
            mapPath = System.Web.Hosting.HostingEnvironment.MapPath("~" + @"\LogInfo\Log.txt");
            StreamWriter sw2 = new StreamWriter(mapPath);
            sw2.WriteLine(log);
            sw2.Close();

            mapPath = System.Web.Hosting.HostingEnvironment.MapPath("~" + @"\LogInfo\LogStart.txt");
            StreamWriter sw3 = new StreamWriter(mapPath);
            sw3.WriteLine("0");
            sw3.Close();
        }

        [WebMethod]
        public string Status() {
            Operation operation = new Operation();
            string mapPath = System.Web.Hosting.HostingEnvironment.MapPath("~" + @"\LogInfo\Log.txt");
            return operation.Status(mapPath);
        }

        [WebMethod]
        public string Process() {
            Operation operation = new Operation();
            string mapPath = System.Web.Hosting.HostingEnvironment.MapPath("~" + @"\LogInfo\LogStart.txt");
            return operation.Status(mapPath);
        }
    }

}
