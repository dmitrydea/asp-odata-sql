using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Service_API.Class
{
    public class Connect
    {
        static string _server = "server_name";
        static string _db = "database_name";
        static string _table = "table_name";
        static SqlConnection con;
        public static bool connect_status = false;

        public static void _connect(string server, string db = "")  //forming of a string connection to a DB
        {
            try
            {
                con = new SqlConnection(@"Data Source = " + _server + @"; Initial Catalog=" + _db + "; Integrated Security=True");
                con.Open();
                connect_status = true;
            }
            catch (Exception)
            {
                connect_status = false;
            }
        }
        public static void Disconnect()
        {
            con.Close();
        }

        

        public static DataTable command_go(string command)
        {
            _connect(_server, _db);
            SqlCommand comm = new SqlCommand(command, con);
            SqlDataReader reader;
            try
            {
                reader  = comm.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return dt;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}