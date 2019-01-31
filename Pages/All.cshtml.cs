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
    public class AllModel : PageModel
    {
        private readonly BirthdayQuery m_birthdayQuery;

        public AllModel(BirthdayQuery birthdayQuery)
        {
            m_birthdayQuery = birthdayQuery;
        }
        public List<Month> Months { get; set; }
        public async Task OnGetAsync(string location)
        {
            Months = await m_birthdayQuery.GetAllBirthdays(location);
            await Task.Delay(0);
        }
    }
}
