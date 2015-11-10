using System.Collections.Generic;
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
    public class Odata_serviseController : ApiController
    {
        Parser parser = new Parser();
        

        [HttpGet]
        public string Get(string request, string field)   //receive answer from GET-request by OData
        {

            return JsonConvert.SerializeObject(parser.process_request(Request.RequestUri.AbsolutePath.Split('/')[3], Request.RequestUri.Query, field));
        }

        public string Get(string request)   //receive answer from GET-request by OData
        {
            
            if (Request.Headers.Accept.ToString() == "application/json")
            {
                return JsonConvert.SerializeObject(parser.process_request(Request.RequestUri.AbsolutePath.Split('/')[3], Request.RequestUri.Query, string.Empty));
            }
            else if (Request.Headers.Accept.ToString() == "application/atom+xml")
            {
                System.IO.StringWriter writer = new System.IO.StringWriter();
                DataTable dt = parser.process_request(Request.RequestUri.AbsolutePath.Split('/')[3], Request.RequestUri.Query, string.Empty);
                dt.WriteXml(writer, XmlWriteMode.WriteSchema, false);
                string result = writer.ToString();
                return result;
            }
            else
            {
                return null;
            }
        }

    }
}
