using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using System.Data.Entity;
using System.Web.OData.Builder;

namespace Service_API.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        //$format=json
        [HttpGet]
        public List<string> GetServers()
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

        SqlConnection con;

        public void Connect(string server)
        {
            con = new SqlConnection(@"Data Source = " + server + @"; Initial Catalog=Material; Integrated Security=True");
            con.Open();
        }

        // GET api/values 
        [HttpGet]
        public IEnumerable<string> GetTables(string server)
        {
            //var data = context.Set<MyEntity>();

            Connect(server);
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
        public List<string> Get(string server, string table)
        {
            Connect(server);
            string SQL_Command_Get_Fields = "SELECT " +
              "COLUMN_NAME"  +
            " FROM   " +
              "INFORMATION_SCHEMA.COLUMNS " +
            "WHERE   " +
              "TABLE_NAME = '" + table + "' " +
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
