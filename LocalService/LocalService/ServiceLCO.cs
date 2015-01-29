using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace LocalService
{
    public partial class ServiceLCO : ServiceBase {
        System.Timers.Timer timr;
        public ServiceLCO() {
            timr = new System.Timers.Timer(1000);
            timr.Elapsed += new System.Timers.ElapsedEventHandler(ExecuteMethod);
            InitializeComponent();
        }
       
        protected override void OnStart(string[] args) {
            timr.Start();
        }

        protected override void OnStop() {
            timr.Stop();
        }

        private void ExecuteMethod(object sender, System.Timers.ElapsedEventArgs e){
            try {
                
                localhost.WebService webService = new localhost.WebService();
                string hourNow = DateTime.Now.ToShortTimeString();
               
                //string hourAppConfig = System.Configuration.ConfigurationManager.AppSettings["hour"];
                //string hourStatus = System.Configuration.ConfigurationManager.AppSettings["hourStatus"];

                if (hourNow == "14:10") {
                    int contador = 1;
                    while(webService.Process() == "0" && contador < 5){
                        webService.UpdateLCO();
                        System.Threading.Thread.Sleep(300000);
                        contador++;
                    } 
                 }

                if (hourNow == "15:10") Status();
             }catch(Exception ex){
                    System.Diagnostics.EventLog.WriteEntry("Aplication", "Exception: " + ex.Message);
            }
        }

        private void Status(){
            localhost.WebService ws = new localhost.WebService();
            System.Diagnostics.EventLog.WriteEntry("Aplication", "EStatus: "  + ws.Status());
        }

      
    }
}
