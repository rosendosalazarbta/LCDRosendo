using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace WebService
{
    public class Connection {
        static private String CadConnection = "Data Source=ACER-PC; Initial Catalog=OPENXMLTesting; Integrated Security=True";
        static private SqlConnection slqConnection;
        static private SqlCommand sqlCommand;
        
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
            try {
                using (SqlConnection sqlconn = new SqlConnection(CadConnection)){
                    sqlconn.Open();
                    sqlCommand = new SqlCommand("SP_XML_TO_SLQ", sqlconn);
                    sqlCommand.CommandTimeout = 0;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@filePath", pathFileXml);

                    sqlCommand.ExecuteNonQuery();  
                }
                return true;
            }catch (SqlException sql) {
                Console.Write(sql);
            }
            
            return false;
        }
    }
}