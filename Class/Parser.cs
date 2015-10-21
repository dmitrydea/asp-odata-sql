﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Service_API.Class
{
    public class Parser
    {
        string table = String.Empty, index = String.Empty, field = String.Empty, query = String.Empty, query_parametr = String.Empty;
        DataTable data_rezult = new DataTable();
        public Parser()
        {
            
        }
        public void a()
        {
            
        }
        public JsonResult process_request(string path, string query)
        {
            Regex find_table = new Regex(@"([A-Za-z_]+)(?:(?=\()||$)(?:(?:\(([1-9+])\))|(?:/|$))(?:/|)([a-zA-Z]+|)(?:[?]|)([a-zA-Z]+|)(?:=|)([A-Z,a-z]+|)");  //выборка таблицы и параметров к ней
            table = find_table.Matches(path + query)[0].Groups[1].ToString();    //table
            index = find_table.Matches(path + query)[0].Groups[2].ToString();    //index
            field = find_table.Matches(path + query)[0].Groups[3].ToString();    //field
            query = find_table.Matches(path + query)[0].Groups[4].ToString();    //query
            query_parametr = find_table.Matches(path + query)[0].Groups[5].ToString();    //query_parametr

            JsonResult jr = new JsonResult();
            if (table != String.Empty)
            {
                process_table();
            }
            else
            {

            }

            return new JsonResult();
        }

        public DataTable process_table()
        {
            ;
            if (index != String.Empty)
            {
                process_index();
            }
            else
            {
                data_rezult = null;
                string SQL_request = "SELECT * FROM \"" + table + "\"";
                data_rezult =  Connect.command_go(SQL_request);
            }
            return data_rezult;
        }
        public DataTable process_index()
        {
            if (field != String.Empty)
            {
                process_field();
            }
            else
            {
                data_rezult = null;
                string SQL_request = "SELECT * FROM \"" + table + "\"  WHERE id = " + index;
                data_rezult = Connect.command_go(SQL_request);
            }
            return data_rezult;
        }
        public void process_field()
        {
            if (query != String.Empty)
            {
                process_query();
            }
        }
        public void process_query()
        {
            
            if (query_parametr != String.Empty)
            {

            }
        }

    }
}