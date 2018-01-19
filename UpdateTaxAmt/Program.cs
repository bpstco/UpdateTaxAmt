using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace UpdateTaxAmt
{
    class Program
    {
        // get connection string from configuration file
        public static string connectionString = ConfigurationManager.AppSettings["connStr"];
        public static string sTimeOut = ConfigurationManager.AppSettings["timeOut"];

        public static StreamWriter swLog;

        static void Main(string[] args)
        {
            int iTimeOut = 60;
            Int32.TryParse(sTimeOut, out iTimeOut);

            swLog = new StreamWriter("TaxAmtRun.txt", true);
            swLog.WriteLine("Start: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            UpdateTaxAmt(iTimeOut);
            swLog.WriteLine("End: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            swLog.Close();
        }

        public static void UpdateTaxAmt(int iTimeOut)
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand cmd = new SqlCommand("dbo.bps_UpdateTaxAmt", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = iTimeOut;
                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}
