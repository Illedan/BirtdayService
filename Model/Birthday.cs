using System;

namespace birthdayservice.Model
{
    public class Birthday
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Years { get; set; }
        public int Days => (int)((Date.Date - DateTime.Now.Date).TotalDays);
        public bool IsAniversary => Years % 10 == 0;
    }
}