using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using birthdayservice.Model;
using birthdayservice.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace birthdayservice.Pages
{
    public class AdminModel : PageModel
    {
        private readonly BirthdayQuery m_birthdayQuery;

        public AdminModel(BirthdayQuery birthdayQuery)
        {
            m_birthdayQuery = birthdayQuery;
        }

        public string Location { get; set; }
        
        public void OnGet(string location)
        {
            Location = location;
        }

        public async Task OnPostBirthday(string location, string name, int day, int month, int year)
        {
            try
            {
                var dt = new DateTime(year, month, day); // Verify date..
                dt.IsDaylightSavingTime(); //keep object.
            }
            catch (Exception exception)
            {
                throw new InvalidEnumArgumentException("Invalid year/month/day");
            }

            await m_birthdayQuery.AddBirthday(
                new BirthAddDto
                {
                    Month = month,
                    Day = day,
                    Year = year,
                    Name = name,
                    Location = location
                });
        }
    }
}
