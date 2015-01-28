using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace WebService
{
    public class Operation {
        private int numFiles;
        private string directory;
        private Connection connection;
        private string log;

        public Operation() {
            directory = @"C:\Users\ACER\Documents\GitHub\LocalService\WebService\Download\";
            numFiles = Directory.GetFiles(directory, "*.gz").Length;
            log = "";
        }

        public string Status(string path){
            string line = "";
            if (File.Exists(path)){
                StreamReader streamReader = new StreamReader(path);
                while ((line = streamReader.ReadLine()) != null)
                {
                    line += line;
                }
                return line;
            }else return "0";
        }
        
        public string Start() {
            Console.WriteLine("Descargando Archivos...");
            DownloadFile();
            if (numFiles < 4) return log;

            Console.WriteLine("Descomprimiendo Archivos...");
            if (!DecompressFiles()) return log;

            Console.WriteLine("Limpiando Archivos...");
            CleanFileXML();

            connection = new Connection();
            Console.WriteLine("Exportando Archivos a la Base de Datos...");
            
            if (ExportXmlToSql() == false) {
                log += "Ha ocurrido un error al tratar de guardar los datos.";
                return log;
            }

            /*
            Console.Clear();
            Console.Write("Ingrese su Numero de Cerfiticado: ");
            ExecuteSelect(Console.ReadLine());
            */

            log =  "Los procesos fueron ejecutados correctamente";
            return log;
        }

        private void DeleteFile() {
            for (int i = 1; i <= numFiles; i++){
                File.Delete(directory + "A" + i + ".gz");
                File.Delete(directory + "A" + i + ".xml");
            }
        }

        private void DownloadFile(){
            DeleteFile();
            bool bandera = true;
            int indice = 1;
            string UriAddress = "http://www.gestionix.com/zip/";
            while (bandera){
                try {
                    String FileName = "A" + indice + ".gz";
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile(UriAddress + FileName, directory + FileName);
                    //Console.WriteLine("->Archivo {0} se ha descargado exitosamente.", FileName);
                    indice++;
                }catch(WebException webex){
                    bandera = false;
                    log = webex.Message;
                }
            }

            numFiles = indice - 1;
        }
       
        private bool DecompressFiles(){
            for (int i = 1; i <= numFiles; i++){
                try {
                    string pathFile = directory + "A" + i + ".gz";
                    FileInfo fileInfo = new FileInfo(pathFile);

                    FileStream fileStream = fileInfo.OpenRead();
                    string namefile = fileInfo.FullName;
                    string newnamefile = namefile.Remove(namefile.Length - fileInfo.Extension.Length);

                    FileStream fileDecompress = File.Create(newnamefile + ".xml");

                    GZipStream decompress = new GZipStream(fileStream, CompressionMode.Decompress);
                    decompress.CopyTo(fileDecompress);
                }
                catch (Exception ex){
                    log += "\n" + ex.Message;
                    return false;
                }
            }
            return true;
        }
       
        private void CleanFileXML(){
            for (int i = 1; i <= numFiles; i++) {
                string fileIn = directory + "A" + i + ".xml";
                string fileOut = directory + "A" + i + "Limpio.xml";

                ExecuteCmd(@"C:\Users\ACER\Documents\GitHub\LocalService\WebService\OpenSSL\bin\openssl smime -decrypt -verify -inform DER -in " + fileIn + " -noverify -out " + fileOut);
                Console.WriteLine("->Archivo {0} de {1} limp9io", i, numFiles);
            }
        }

        private void ExecuteCmd(string command) {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd", "/c " + command);
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = false;

            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();

            process.StandardOutput.ReadToEnd();
        }
        
        private bool ExportXmlToSql() {
            for (int i = 1; i <= numFiles; i++) {
                if (connection.StoredXmlToQql("FILE" + i) == false) return false;
                Console.WriteLine("Archivo Exportado. {0} de {1}", i, numFiles);
            }
            return true;
        }
        
        private void ExecuteSelect(string ncertificado) {
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
    }

}