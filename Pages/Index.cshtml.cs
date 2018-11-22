using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using birthdayservice.Model;
using birthdayservice.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace birthdayservice.Pages
{
    public class IndexModel : PageModel
    {
        private readonly BirthdayQuery m_birthdayQuery;

        public IndexModel(BirthdayQuery birthdayQuery)
        {
            m_birthdayQuery = birthdayQuery;
        }

        public BirtdayResponse Response { get; set; }
        public async Task OnGetAsync(string location)
        {
            var birtdays = await m_birthdayQuery.GetBirthdays(location);
            await Task.Delay(0);
            var now = DateTime.Now;
            Response = new BirtdayResponse
            {
                TodaysBirthdays = birtdays.Where(b => b.Date.Day == now.Day && b.Date.Month == now.Month).ToList()
            };

            Response.NextBirthdays.AddRange(birtdays.Where(b => !Response.TodaysBirthdays.Contains(b)).Take(3));
        }
    }
}
