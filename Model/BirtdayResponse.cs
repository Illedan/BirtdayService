using System.Collections.Generic;

namespace birthdayservice.Model
{
    public class BirtdayResponse
    {
        public List<Birthday> TodaysBirthdays { get; set; }
        public List<Birthday> NextBirthdays { get; } = new List<Birthday>();
    }
}