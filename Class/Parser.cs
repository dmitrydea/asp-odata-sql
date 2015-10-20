using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Service_API.Class
{
    public class Parser
    {
        string table;
        public Parser()
        {
            
        }
        public void a()
        {
            
        }
        public List<string> process_request(string path, string query)
        {
            List<string> list = new List<string>();
            Regex find_table = new Regex(@"^([^(\/]+)(?:\((.*?)\))?\/(?:([^?]+))?\?select=(.*)$");  //выборка таблицы и параметров к ней
            list.Add("table=" + find_table.Matches(path + query)[0].ToString());    //table
            list.Add("table=" + find_table.Matches(path + query)[1].ToString());    //index
            list.Add("table=" + find_table.Matches(path + query)[2].ToString());    //field
            list.Add("table=" + find_table.Matches(path + query)[3].ToString());    //field
            return list;
        }
    }
}