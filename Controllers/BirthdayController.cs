using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using birthdayservice.Model;
using birthdayservice.Query;
using Microsoft.AspNetCore.Mvc;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace birthdayservice.Controllers
{
    [Produces("application/json")]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class BirthdayController : Controller
    {
        private readonly BirthdayQuery m_birthdayQuery;

        public BirthdayController(BirthdayQuery birthdayQuery)
        {
            m_birthdayQuery = birthdayQuery;
        }
        

        [Microsoft.AspNetCore.Mvc.HttpGet("{location}/{amount?}")]
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

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> Post([FromBody]BirthdayAddRequest birthdayAddRequest)
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

            return StatusCode(200);
        }


        [Microsoft.AspNetCore.Mvc.HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await m_birthdayQuery.DeleteBirthday(id);
            return StatusCode(200);
        }
    }
}
