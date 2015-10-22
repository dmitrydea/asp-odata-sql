using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web.Http;
using Service_API.Class;
using Newtonsoft.Json;

namespace Service_API.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        Parser parser = new Parser();

        [HttpGet]
        public string Get(string request, string field)   //получение GET-запроса по стандарту OData
        {
            return JsonConvert.SerializeObject(parser.process_request(Request.RequestUri.AbsolutePath.Split('/')[3], Request.RequestUri.Query));
        }

        public string Get(string request)   //получение GET-запроса по стандарту OData
        {
            return JsonConvert.SerializeObject(parser.process_request(Request.RequestUri.AbsolutePath.Split('/')[3], Request.RequestUri.Query));
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
