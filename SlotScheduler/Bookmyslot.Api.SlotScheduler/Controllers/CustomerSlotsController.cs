using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookmyslot.Api.SlotScheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerSlotsController : ControllerBase
    {
        // GET: api/<CustomerSlotsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CustomerSlotsController>/5
        [HttpGet("{email}")]
        public string Get(string email)
        {
            return "value";
        }

        // POST api/<CustomerSlotsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CustomerSlotsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CustomerSlotsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
