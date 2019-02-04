using System;

namespace birthdayservice.Model
{
    public class Birthday
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Years { get; set; }
        public int Days { get; set; }
        public bool IsAniversary => Years % 10 == 0;
    }
}