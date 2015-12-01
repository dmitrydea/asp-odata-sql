using System;
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
        string table = String.Empty, index = String.Empty, field = String.Empty;
        string[]query = new string[0], query_parametr = new string[0];
        DataTable data_rezult = new DataTable();
        public Parser()
        {

        }
        public DataTable process_request(string path, string query, string field)
        {
            query =  HttpUtility.UrlDecode(query).Replace("\"", "'");
            Regex path_reg = new Regex(@"([A-Za-z_]+)(?:(?=\()||$)(?:(?:\(([1-9+])\))|(?:/||$))(?:/||$)([a-zA-Z]+||$)");  //receipt of a name of the table, fields and indexes to them
            Regex query_reg = new Regex(@"(?:[?]|)(?:([\$a-zA-Z]+|)(?:=|)([\*1-9a-zA-Zа-яА-Я, ']+||$)|([\$&a-zA-Z]+|)(?:=|)([\*1-9a-zA-Zа-яА-Я, ']+||$)|([\$&a-zA-Z]+|)(?:=|)([A-Za-z_]+)(?:\(([\$a-zA-Z]+|)(?:=|)([\*1-9a-zA-Zа-яА-Я, ']+||$)\)))"); //receipt of a line of request and parameters
            try
            {
                table = path_reg.Matches(path + query)[0].Groups[1].ToString();    //table name
                index = path_reg.Matches(path + query)[0].Groups[2].ToString();    //index
                var matches = query_reg.Matches(query);
                if (matches[0].Groups[1].ToString() != "")
                {
                    Array.Resize(ref this.query, this.query.Length + 1);
                    Array.Resize(ref query_parametr, query_parametr.Length + 1);
                    this.query[this.query.Length - 1] = matches[0].Groups[1].ToString();    //query_first(expand)
                    query_parametr[query_parametr.Length - 1] = matches[0].Groups[2].ToString();    //query_parametr
                }
                for (int i = 1; i < matches.Count; i++)
                {
                    string s = matches[i].Groups[1].ToString();
                    if (matches[i].Groups[1].ToString() != "")
                    {
                        
                        if (matches[i].Groups[1].ToString().IndexOf("$expand") != -1)
                        {
                            Array.Resize(ref this.query, this.query.Length + 1);
                            Array.Resize(ref query_parametr, query_parametr.Length + 1);
                            this.query[this.query.Length - 1] = matches[i].Groups[1].ToString();    //query_first(expand)
                            this.query[this.query.Length - 1] += matches[i + 2].Groups[1].ToString();    //query_second
                            query_parametr[query_parametr.Length - 1] = matches[i + 2].Groups[2].ToString();    //query_parametr
                            i += 2;
                        }
                        else
                        {
                            Array.Resize(ref this.query, this.query.Length + 1);
                            Array.Resize(ref query_parametr, query_parametr.Length + 1);
                            this.query[this.query.Length - 1] = matches[i].Groups[1].ToString();    //query
                            query_parametr[query_parametr.Length - 1] = matches[i].Groups[2].ToString();    //query_parametr
                        }
                    }
                    
                }
            }
            catch
            {
                return null;
            }

            this.field = field;    //field

            if (table != String.Empty)
            {
                 return process_table();
            }
            else
            {
                return null;
            }
        }

        public DataTable process_table()    //processing the request to output of tables
        {
            if (query.Length != 0)
            {
                return process_query();
            }
            else if (index != String.Empty)
            {
                return process_index();
            }
            else if (field != String.Empty)
            {
                return process_field();
            }
            else
            {
                data_rezult = null;
                string SQL_request = "SELECT * FROM \"" + table + "\"";
                data_rezult =  Connect.command_go(SQL_request);
                if (data_rezult != null)
                {
                    data_rezult.TableName = table;
                }
            }
            return data_rezult;
        }
        public DataTable process_index()    
        {
            if (field != String.Empty)
            {
                return process_field();
            }
            else if (query.Length != 0)
            {
                return process_query();
            }
            else
            {
                data_rezult = null;
                string SQL_request = "SELECT * FROM \"" + table + "\"  WHERE id = " + index;
                data_rezult = Connect.command_go(SQL_request);
                if (data_rezult != null)
                {
                    data_rezult.TableName = table;
                }
            }
            return data_rezult;
        }
        public DataTable process_field()
        {
            if (query.Length != 0)
            {
                process_query();
            }
            else
            {
                data_rezult = null;
                string SQL_request = "SELECT " + field + " FROM \"" + table + "\"";
                data_rezult = Connect.command_go(SQL_request);
                if (data_rezult != null)
                {
                    data_rezult.TableName = table;
                }

            }
            return data_rezult;
        }
        /*
         * SELECT HID.ToString() AS Text_OrgNode, 
HID.GetLevel() AS EmpLevel, *
FROM dbo.MaterialClass
WHERE HID.GetLevel() = 0;
         * */
        public DataTable process_query()
        {
            if (query_parametr.Length != 0)
            {
                data_rezult = null;
                string SQL_request = string.Empty;
                for (int i = 0; i < query.Length; i++)
                {

                    switch (query[i])
                    {
                        /*case "$search":
                            SQL_request = "SELECT * FROM \"" + table + "\" WHERE " + query_parametr;
                            break;*/
                        case "$filter":
                            query_parametr[i] = query_parametr[i].Replace("and", "&").Replace("or", "|").Replace("lt", "<").Replace("gt", ">").Replace("eq", "=").Replace("ne", "!=").Replace("ge", ">=").Replace("le", "<=").Replace("ne", "<>");
                            SQL_request += "SELECT " + (field == String.Empty ? "*" : field) + " FROM \"" + table + "\" WHERE " + query_parametr[i];
                            break;
                        /*case "$count":
                            //SQL_request = "SELECT top " + query_parametr + " * FROM \"" + table + "\"";
                            break;
                        case "$orderby":
                            //SQL_request = "SELECT top " + query_parametr + " * FROM \"" + table + "\"";
                            break;*/
                        case "$skip":
                            SQL_request += "SELECT " + (field == String.Empty ? "*" : field) + " FROM  ( select *, ROW_NUMBER() over (ORDER BY id) AS ROW_NUM from \"" + table + "\") x where ROW_NUM>" + query_parametr[i];
                            break;
                        case "$top":
                            SQL_request += "SELECT top " + query_parametr[i] + " * FROM \"" + table + "\"";
                            break;
                        case "$select":
                            SQL_request += "SELECT " + query_parametr[i] + " FROM \"" + table + "\"";
                            break;
                        default:
                            if (index != String.Empty)
                            {
                                SQL_request += query[i] + " " + query_parametr[i] + " FROM \"" + table + "\" WHERE id = " + index;
                            }
                            else
                            {
                                SQL_request += query[i] + " " + query_parametr[i] + " FROM \"" + table + "\"";
                            }
                            break;
                    }

                    if (i + 1 != query.Length && query[i+1].IndexOf("$expand") != -1)
                    {
                        query[i + 1] = query[i + 1].Replace("$expand", "");
                        SQL_request += " UNION ";
                    }

                }
                data_rezult = Connect.command_go(SQL_request);
                if (data_rezult != null)
                {
                    data_rezult.TableName = table;
                }
            }
            return data_rezult;
        }

    }
}