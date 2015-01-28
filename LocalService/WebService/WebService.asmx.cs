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
            return "Hello World" + "....";
        }

        [WebMethod]
        public void UpdateLCO(){
            Operation operation = new Operation();
            StreamWriter sw = new StreamWriter(@"C:\Users\ACER\Documents\GitHub\LocalService\WebService\LogInfo\LogStart.txt");
            sw.WriteLine("En Ejecución...");
            sw.Close();
            
            string log = operation.Start();
            StreamWriter sw2 = new StreamWriter(@"C:\Users\ACER\Documents\GitHub\LocalService\WebService\LogInfo\Log.txt");
            sw2.WriteLine(log);
            sw2.Close();

            StreamWriter sw3 = new StreamWriter(@"C:\Users\ACER\Documents\GitHub\LocalService\WebService\LogInfo\LogStart.txt");
            sw3.WriteLine("0");
            sw3.Close();
        }

        [WebMethod]
        public string Status() {
            Operation operation = new Operation();
            return operation.Status(@"C:\Users\ACER\Documents\GitHub\LocalService\WebService\LogInfo\Log.txt");
        }

        [WebMethod]
        public string Process() {
            Operation operation = new Operation();
            return operation.Status(@"C:\Users\ACER\Documents\GitHub\LocalService\WebService\LogInfo\LogStart.txt");
        }
    }

}
