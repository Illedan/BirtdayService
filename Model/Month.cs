using System.Collections.Generic;

namespace birthdayservice.Model
{
    public class Month
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public List<Birthday> Birthdays { get; set; }
    }
}