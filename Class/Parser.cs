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
            Regex find_table = new Regex(@"([A-Za-z_]+)(?:(?=\()||$)(?:(?:\(([1-9+])\))|(?:/|$))(?:/|)([a-zA-Z]+|)(?:[?]|)([a-zA-Z]+|)(?:=|)([A-Z,a-z]+|)");  //выборка таблицы и параметров к ней
            var mmmm = find_table.Matches(path + query);
            list.Add("table=" + find_table.Matches(path + query)[0].Groups[1].ToString());    //table
            list.Add("index=" + find_table.Matches(path + query)[0].Groups[2].ToString());    //index
            list.Add("field=" + find_table.Matches(path + query)[0].Groups[3].ToString());    //field
            list.Add("query=" + find_table.Matches(path + query)[0].Groups[4].ToString());    //query
            list.Add("query_parametr=" + find_table.Matches(path + query)[0].Groups[5].ToString());    //query_parametr
            return list;
        }
    }
}