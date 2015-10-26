﻿using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web.Http;
using Service_API.Class;
using Newtonsoft.Json;
using System.IO;

namespace Service_API.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        Parser parser = new Parser();
        

        [HttpGet]
        public string Get(string request, string field)   //receive answer from GET-request by OData
        {
            
            return JsonConvert.SerializeObject(parser.process_request(Request.RequestUri.AbsolutePath.Split('/')[3], Request.RequestUri.Query));
        }

        public string Get(string request)   //receive answer from GET-request by OData
        {
            
            if (Request.Headers.Accept.ToString() == "application/json")
            {
                return JsonConvert.SerializeObject(parser.process_request(Request.RequestUri.AbsolutePath.Split('/')[3], Request.RequestUri.Query));
            }
            else if (Request.Headers.Accept.ToString() == "application/atom+xml")
            {
                System.IO.StringWriter writer = new System.IO.StringWriter();
                DataTable dt = parser.process_request(Request.RequestUri.AbsolutePath.Split('/')[3], Request.RequestUri.Query);
                dt.TableName = "a";
                dt.WriteXml(writer, XmlWriteMode.WriteSchema, false);
                string result = writer.ToString();
                return result;
            }
            else
            {
                return null;
            }
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