using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web.Http;
using Service_API.Class;

namespace Service_API.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        Parser parser = new Parser();
        string _server = "Alexey-PC";
        string _db = "Material";
        string _table = "user";
        SqlConnection con;

        [HttpGet]
        public List<string> Get(string request, string field)   //получение GET-запроса по стандарту OData
        {
            return parser.process_request(Request.RequestUri.AbsolutePath.Split('/')[3], Request.RequestUri.Query);
            
            return new List<string>();
        }

        public List<string> Get(string request)   //получение GET-запроса по стандарту OData
        {
            return parser.process_request(Request.RequestUri.AbsolutePath.Split('/')[3], Request.RequestUri.Query);

            return new List<string>();
        }

        public void Connect(string server, string db = "")  //формирование строки подключение к БД
        {
            con = new SqlConnection(@"Data Source = " + _server + @"; Initial Catalog=" + _db + "; Integrated Security=True");
            con.Open();
        }

        [HttpGet]
        public List<string> GetServers() //получение всех серверов доступных компьютеру
        {
            SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
            DataTable dt =  instance.GetDataSources();
            List<string> servers = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                servers.Add(dt.Rows[i][0].ToString());
            }
            return servers;
            
        }

        [HttpGet]
        public IEnumerable<string> GetDB(string server) //получение всех БД с сервера server
        {

            Connect(server);
            string result = string.Empty;
            string SQL_Command_Get_Tables = "EXEC sp_Databases";
            SqlCommand comm = new SqlCommand(SQL_Command_Get_Tables, con);
            SqlDataReader reader = comm.ExecuteReader();
            List<string> table = new List<string>();
            while (reader.Read())
            {
                table.Add(reader.GetString(0));
            }

            con.Close();
            reader.Close();
            return table;
        }

        [HttpGet]
        public IEnumerable<string> GetTables(string server, string db) //получение всех таблиц с базы данных db
        {

            Connect(_server, _db);
            string result = string.Empty;
            string SQL_Command_Get_Tables = "SELECT TABLE_NAME FROM information_schema.TABLES";
            SqlCommand comm = new SqlCommand(SQL_Command_Get_Tables, con);
            SqlDataReader reader = comm.ExecuteReader();
            List<string> table = new List<string>();
            while (reader.Read())
            {
                table.Add(reader.GetString(0));
            }

            con.Close();
            reader.Close();
            return table;
        }

        [HttpGet]
        public List<string> Get_info(string server, string table) //получение всех полей, которые есть в таблице table
        {
            Connect(_server);
            string SQL_Command_Get_Fields = "SELECT " +
              "COLUMN_NAME"  +
            " FROM   " +
              "INFORMATION_SCHEMA.COLUMNS " +
            "WHERE   " +
              "TABLE_NAME = '" + _table + "' " +
            "ORDER BY " +
              "ORDINAL_POSITION ASC; ";

            SqlCommand comm = new SqlCommand(SQL_Command_Get_Fields, con);
            SqlDataReader reader = comm.ExecuteReader();
            List<string> colums_name = new List<string>();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    colums_name.Add(reader.GetValue(i).ToString());
                }
            }
            con.Close();
            con.Dispose();
            return colums_name;
        }

        
        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
