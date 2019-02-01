using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using birthdayservice.Model;
using birthdayservice.Query;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace birthdayservice.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class BirthdayController : Controller
    {
        private readonly BirthdayQuery m_birthdayQuery;

        public BirthdayController(BirthdayQuery birthdayQuery)
        {
            m_birthdayQuery = birthdayQuery;
        }
        

        // GET: api/<controller>
        [HttpGet("{location}")]
        public async Task<BirtdayResponse> Get(string location)
        {
            var birtdays = await m_birthdayQuery.GetBirthdays(location);
            var now = DateTime.Now;
            var response = new BirtdayResponse
            {
                TodaysBirthdays = birtdays.Where(b => b.Date.Day == now.Day && b.Date.Month == now.Month).ToList()
            };

            response.NextBirthdays.AddRange(birtdays.Where(b => !response.TodaysBirthdays.Contains(b)).Take(3));
            return response;
        }

        //// GET api/<controller>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
