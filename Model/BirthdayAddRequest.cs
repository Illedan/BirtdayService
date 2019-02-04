using System;

namespace birthdayservice.Model
{
    public class BirthdayAddRequest
    {
        public string Location { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}