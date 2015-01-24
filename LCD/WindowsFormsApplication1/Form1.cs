using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Linq;




namespace WindowsFormsApplication1
{
    public partial class Form1 : Form {

        private clsConnectDB Connection;
        private int NumberFiles;

        public Form1(){
            InitializeComponent();
            Connection = new clsConnectDB();
        
            //NumberFiles = FileSystem.GetFiles(@"C:\Users\ACER\Documents\LCO\").Count;
        }

        private void Form1_Load(object sender, EventArgs e){
            ReadXMLDocument();
            //DownloadFile();
        }


        private void DeleteFile(){
            for(int i = 1; i<= NumberFiles; i++){
                File.Delete( @"C:\Users\ACER\Documents\LCO\A" + i + ".gz");
            }
        }

        private void DownloadFile(){
            //DeleteFile();
            String UriAddress = "http://www.gestionix.com/zip/";
            for (int i = 1; i < 5; i++){
                String FileName = "A" +  i + ".gz";
                WebClient webClient = new WebClient();
                webClient.DownloadFile((UriAddress + FileName), FileName);
            }
        }

        private void ReadXMLDocument() {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(@"C:\Users\ACER\Documents\LCO.xml");

            XmlNodeList ParentNodeList = xmlDocument.GetElementsByTagName("lco:Contribuyente");

            foreach (XmlElement xmlElement in ParentNodeList){
                string rfc = xmlElement.GetAttribute("RFC");

                XmlNodeList nodes = xmlElement.GetElementsByTagName("lco:Certificado");

                foreach (XmlElement node in nodes) {
                    string ValidezObligaciones = node.GetAttribute("ValidezObligaciones");
                    string EstatusCertificado = node.GetAttribute("EstatusCertificado");
                    string noCertificado = node.GetAttribute("noCertificado");
                    string FechaInicio = node.GetAttribute("FechaInicio");
                    string FechaFinal = node.GetAttribute("FechaFinal");

                    //Connection.StoredProcedureClient(rfc, ValidezObligaciones, EstatusCertificado, noCertificado, FechaInicio, FechaFinal);
                    
                }
            }
            MessageBox.Show("Insertando...");

          
        }
            
        private void ExportXMLToSQLServer(){
            Connection = new clsConnectDB();

        }
    }
}
