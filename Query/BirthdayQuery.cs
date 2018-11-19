using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using birthdayservice.Model;

namespace birthdayservice.Query
{
    public class BirthdayQuery
    {
        public BirthdayQuery()
        {
            
        }

        public async Task<List<Birthday>> GetBirthdays(string location)
        {
            if (location.Contains(";")) throw new ArgumentException("Invalid location");
            string cs = "URI=file:" + DatabaseFileLocation.ConnectString;
            var birthdays = new List<Birthday>();
            using (SQLiteConnection con = new SQLiteConnection(cs))
            {
                con.Open();
                var month = DateTime.Now.Month;
                var nextMonth = month + 1;
                if (nextMonth > 12) nextMonth = 1;

                //CREATE TABLE birthday (id integer primary key, location VARCHAR(50), name VARCHAR(50),
                //birthday int, birthmonth int, birthyear int, deleted boolean, deletedreason varchar(100));
                string stm = $"SELECT * FROM birthday where location = '{location}' COLLATE NOCASE and (birthmonth like {month} or birthmonth like {nextMonth})";
                using (SQLiteCommand cmd = new SQLiteCommand(stm, con))
                {
                    var rdr = await cmd.ExecuteReaderAsync();
                    while (rdr.Read())
                    {
                        var date = new DateTime(rdr.GetInt32(5), rdr.GetInt32(4), rdr.GetInt32(3));
                        var note = new Birthday
                        {
                            //Id = rdr.GetInt32(0),
                            Name = rdr.GetString(2),
                            Date = new DateTime(date.Month == nextMonth && nextMonth == 1 ? DateTime.Now.Year + 1 : DateTime.Now.Year, date.Month, date.Day), 
                            Years = (DateTime.Now.Year-date.Year)
                        };

                        birthdays.Add(note);
                    }

                    rdr.Dispose();
                }

                con.Close();
            }

            birthdays = birthdays.Where(b=> b.Date.Date >= DateTime.Now.Date).OrderBy(b => b.Days).ToList();
            return birthdays;
        }

    }
}