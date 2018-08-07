using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FDAPI.Models;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FDAPI.Controllers
{
    [Route("aircraft")]
    [ApiController]
    public class DuelistController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value" + id.ToString();
        }

        // POST aircraft/duelist/1
        [HttpPost("{id}/duelist")]
        public string Post(int id, [FromBody]List<FDTask> value){
            IO input = new IO();
            input.aircraftID = id;

            List<FDTask> theTasks = value;
            input.fDTasks = theTasks;

            string toSend = JsonConvert.SerializeObject(input);

            RabbitInteract mq = new RabbitInteract();


            string theResponse = mq.Rpc(toSend);
            
            return theResponse;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            
        }
    }
}
