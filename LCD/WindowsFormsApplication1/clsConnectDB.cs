using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace WindowsFormsApplication1
{
    class clsConnectDB{
        private String CadenaConexion = "Data Source=ACER-PC; Initial Catalog=OPENXMLTesting; Integrated Security=True";
        private SqlConnection slqConnection;
        private SqlCommand sqlCommand;
        public bool ConnectDB(){
            try {
                slqConnection = new SqlConnection(CadenaConexion);
                slqConnection.Open();
                return true;
            }
            catch (SqlException sqlexeption) {
                Console.Write(sqlexeption);
                return false;
            }
        }

        public DataTable getConsulta(String sentenceSQL) {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sentenceSQL, slqConnection);
            da.Fill(dt);
            CloseConnection();
            return dt;
        }

        private void CloseConnection(){
            slqConnection.Close();
        }

        public bool StoredProcedureClient(params string[] param){
            try
            {
                ConnectDB();
                sqlCommand = new SqlCommand("SP_CLIENT", slqConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Rfc", param[0]);
                sqlCommand.Parameters.AddWithValue("@ValidezObligaciones", param[1]);
                sqlCommand.Parameters.AddWithValue("@EstatusCertificado ", param[2]);
                sqlCommand.Parameters.AddWithValue("@noCertificado", param[3]);
                sqlCommand.Parameters.AddWithValue("@FechaInicio", param[4]);
                sqlCommand.Parameters.AddWithValue("@FechaFinal", param[5]);

                sqlCommand.ExecuteNonQuery();
                CloseConnection();
                return true;
            }
            catch (Exception e) {
                return false;
            }
        }
    }
}
