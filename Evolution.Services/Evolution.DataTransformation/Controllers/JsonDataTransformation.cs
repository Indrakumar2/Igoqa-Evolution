using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Evolution.DataTransformation.Controllers
{
    /// <summary>
    /// This will take care to merge different json into single json
    /// </summary>
    [Route("api/datatransformation/json")]
    [ApiController]
    public class JsonDataTransformation : ControllerBase
    {
        // POST api/datatransformation/json
        [HttpPost]
        public string Post([FromBody] string[] jsonString,string jsonTemplate)
        {
            return "Success";
        }       
    }
}
