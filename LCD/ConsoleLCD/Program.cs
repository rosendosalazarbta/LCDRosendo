using System;

using System.Collections.Generic;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows;
using System.Xml;




namespace ConsoleLCD
{
    class Program
    {
        static string directory; 
        static int NumberFiles;
        static Connection connection;

        static void Main(string[] args){
            directory = @"C:\Users\ACER\Documents\GitHub\LCO\";
            NumberFiles = Directory.GetFiles(directory, "*.gz").Length;
            Console.WriteLine("Descargando Archivos...");
            DownloadFile();
            Console.WriteLine("Descomprimiendo Archivos...");
            DecompressFiles();
            Console.WriteLine("Limpiando Archivos...");
            CleanFileXML();
            
            connection = new Connection();
            Console.WriteLine("Exportando Archivos a la Base de Datos...");
            ExportXmlToSql();
            Console.Clear();
            Console.Write("Ingrese su Numero de Cerfiticado: ");
            ExecuteSelect(Console.ReadLine());
           
            Console.ReadKey();
           
        }

        static void DeleteFile() {
            for (int i = 1; i <= NumberFiles; i++){
                File.Delete(directory + "A" + i + ".gz");
                File.Delete(directory + "A" + i + ".xml");
            }
        }

        static void DownloadFile(){
            DeleteFile();
            bool bandera = true;
            int cont = 1;
            string UriAddress = "http://www.gestionix.com/zip/";
            while (bandera){
                try {
                    String FileName = "A" + cont + ".gz";
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile(UriAddress + FileName, directory + FileName);
                    Console.WriteLine("->Archivo {0} se ha descargado exitosamente.", FileName);
                    cont++;
                }catch(WebException webex){
                    bandera = false;
                }
            }

            NumberFiles = cont - 1;
        }

        static void DecompressFiles(){
            for (int i = 1; i <= NumberFiles; i++){
                string pathFile = directory + "A" + i + ".gz";
                //DirectoryInfo dirInfo = new DirectoryInfo(pathFile);
                FileInfo fileInfo = new FileInfo(pathFile);

                FileStream fileStream = fileInfo.OpenRead();
                string namefile = fileInfo.FullName;
                string newnamefile = namefile.Remove(namefile.Length - fileInfo.Extension.Length);

                FileStream fileDecompress = File.Create(newnamefile + ".xml");

                GZipStream decompress = new GZipStream(fileStream, CompressionMode.Decompress);
                decompress.CopyTo(fileDecompress);
            }
        }

        static void CleanFileXML(){
            for (int i = 1; i <= NumberFiles; i++) {
                string fileIn = directory + "A" + i + ".xml";
                string fileOut = directory + "A" + i + "Limpio.xml";

                ExecuteCmd(@"C:\Users\ACER\Documents\GitHub\OpenSSL\bin\openssl smime -decrypt -verify -inform DER -in " + fileIn + " -noverify -out " + fileOut);
                Console.WriteLine("->Archivo {0} de {1} limpio", i, NumberFiles);
            }
        }

        static void ExecuteCmd(string command) {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd", "/c " + command);
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = false;

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo = processStartInfo;
            process.Start();

            process.StandardOutput.ReadToEnd();
        }
        
        static void ExportXmlToSql() {
            for (int i = 1; i <= NumberFiles; i++) {
                connection.StoredXmlToQql(/*@"C:\Users\ACER\Documents\GitHub\LCO\A" + i + "Limpio.xml"*/ "FILE" + i);
                Console.WriteLine("Archivo Exportado. {0} de {1}", i, NumberFiles);
            }
        }

        static void ExecuteSelect(string ncertificado) {
            string sql = "SELECT * FROM Certificado WHERE noCertificado='"+ ncertificado + "'";
            DataTable dt = connection.ExecuteSelect(sql);

            if (dt.Rows.Count > 0){
                Console.WriteLine("**RFC: " + dt.Rows[0][0].ToString());
                Console.WriteLine("**Validez Obligaciones: " + dt.Rows[0][1].ToString());
                Console.WriteLine("**Estatus Certificado: " + dt.Rows[0][2].ToString());
                Console.WriteLine("**Fecha Inicio: " + dt.Rows[0][4].ToString());
                Console.WriteLine("**Fecha Final: " + dt.Rows[0][4].ToString());
            }
            else Console.WriteLine("0 Registros Encontrados.");
        }

        static void ReadXMLDocument()
        {
            
            DataTable dt = new DataTable();

            dt.Columns.Add("Rfc");
            dt.Columns.Add("ValidezObligaciones");
            dt.Columns.Add("EstatusCertificado");
            dt.Columns.Add("noCertificado");
            dt.Columns.Add("FechaInicio");
            dt.Columns.Add("FechaFinal");

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(@"C:\Users\ACER\Documents\LCO.xml");

            XmlNodeList ParentNodeList = xmlDocument.GetElementsByTagName("lco:Contribuyente");

            foreach (XmlElement xmlElement in ParentNodeList)
            {
                string rfc = xmlElement.GetAttribute("RFC");

                XmlNodeList nodes = xmlElement.GetElementsByTagName("lco:Certificado");

                foreach (XmlElement node in nodes)
                {
                    DataRow row = dt.NewRow();
                    row["Rfc"] = rfc;
                    row["ValidezObligaciones"] = node.GetAttribute("ValidezObligaciones");
                    row["EstatusCertificado"] = node.GetAttribute("EstatusCertificado");
                    row["noCertificado"] = node.GetAttribute("noCertificado");
                    row["FechaInicio"] = node.GetAttribute("FechaInicio");
                    row["FechaFinal"] = node.GetAttribute("FechaFinal");

                    dt.Rows.Add(row);
                    //listClient.Add(new Client(rfc, ValidezObligaciones, EstatusCertificado, noCertificado, FechaInicio, FechaFinal));
                    //Connection.StoredProcedureClient(rfc, ValidezObligaciones, EstatusCertificado, noCertificado, FechaInicio, FechaFinal);
                    
                }
            }
            int x = dt.Rows.Count;
        }
    }



    class Connection {

        private String CadConnection = "Data Source=ACER-PC; Initial Catalog=OPENXMLTesting; Integrated Security=True";
        private SqlConnection slqConnection;
        private SqlCommand sqlCommand;
        
        private bool OpenConnection(){
            try {
                slqConnection = new SqlConnection(CadConnection);
                slqConnection.Open();
                return true;
            }
            catch (SqlException sqlexeption) {
                Console.Write(sqlexeption);
                return false;
            }
        }

        public DataTable ExecuteSelect(String sentenceSQL) {
            OpenConnection();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sentenceSQL, slqConnection);
            da.Fill(dt);
            CloseConnection();
            return dt;
        }

        private void CloseConnection(){
            slqConnection.Close();
        }

        public bool StoredXmlToQql(string pathFileXml){
            OpenConnection();
            try {
                sqlCommand = new SqlCommand("SP_XML_TO_SLQ", slqConnection);
                sqlCommand.CommandTimeout = 0;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@filePath", pathFileXml);

                sqlCommand.ExecuteNonQuery();
                CloseConnection();
                return true;
            }catch (SqlException sql) {
                CloseConnection();
                Console.Write(sql);
            }
            
            return false;
        }
       
    }
}
