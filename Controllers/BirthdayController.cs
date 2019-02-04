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
        [HttpGet("{location}/{amount?}")]
        public async Task<BirtdayResponse> Get(string location, int amount=3)
        {
            var birtdays = await m_birthdayQuery.GetBirthdays(location);
            var now = DateTime.Now;
            var response = new BirtdayResponse
            {
                TodaysBirthdays = birtdays.Where(b => b.Days == 0).ToList()
            };
            if (response.TodaysBirthdays.Count < amount)
            {
                response.NextBirthdays.AddRange(birtdays.Where(b => !response.TodaysBirthdays.Contains(b)).Take(amount - response.TodaysBirthdays.Count));
            }

            return response;
        }

        //// GET api/<controller>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        [HttpPost]
        public async Task Post([FromBody]BirthdayAddRequest birthdayAddRequest)
        {
            await m_birthdayQuery.AddBirthday(
                new BirthAddDto
                {
                    Name = birthdayAddRequest.Name,
                    Location = birthdayAddRequest.Location,
                    Day = birthdayAddRequest.Date.Day,
                    Month = birthdayAddRequest.Date.Month,
                    Year = birthdayAddRequest.Date.Year,
                });
        }

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
