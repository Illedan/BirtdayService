using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using birthdayservice.Model;
using birthdayservice.Utilities;

namespace birthdayservice.Query
{
    public class BirthdayQuery
    {
        public async Task<List<Birthday>> GetBirthdays(string location)
        {
            if (location.Any(l=> !char.IsNumber(l) && !char.IsLetter(l))) throw new ArgumentException("Invalid location");
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
                        var year = date.Month == nextMonth && nextMonth == 1 ? DateTime.Now.Year + 1 : DateTime.Now.Year;
                        var note = new Birthday
                        {
                            Id = rdr.GetInt32(0),
                            Name = rdr.GetString(2),
                            Date = date,
                            Days = (int)((new DateTime(year, date.Month, date.Day).Date - DateTime.Now.Date).TotalDays),
                            Years = (DateTime.Now.Year-date.Year)
                        };

                        birthdays.Add(note);
                    }

                    rdr.Dispose();
                }

                con.Close();
            }

            birthdays = birthdays.Where(b=> b.Days >= 0).OrderBy(b => b.Days).ToList();
            return birthdays;
        }

        public async Task<List<Month>> GetAllBirthdays(string location)
        {
            if (string.IsNullOrEmpty(location) || location.Any(l => !char.IsNumber(l) && !char.IsLetter(l))) throw new ArgumentException("Invalid location");
            string cs = "URI=file:" + DatabaseFileLocation.ConnectString;
            var birthdays = new List<Birthday>();
            using (SQLiteConnection con = new SQLiteConnection(cs))
            {
                con.Open();

                string stm = $"SELECT * FROM birthday where location = '{location}' COLLATE NOCASE";
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
                            Date = date,
                            Years = (DateTime.Now.Year-date.Year)
                        };

                        birthdays.Add(note);
                    }

                    rdr.Dispose();
                }

                con.Close();
            }

            return birthdays.GroupBy(b => b.Date.Month).Select(
                    b => new Month { Birthdays = b.OrderBy(day => day.Date.Day).ToList(), Name = b.First().Date.Month.GetMonthName(), Number = b.First().Date.Month })
                .OrderBy(b => b.Number).ToList();
        }

        public async Task AddBirthday(BirthAddDto birthAddDto)
        {
            if (birthAddDto.Location.Any(l => !char.IsNumber(l) && !char.IsLetter(l))) throw new ArgumentException("Invalid location");
            if (birthAddDto.Name.Any(l => !char.IsNumber(l) && !char.IsLetter(l) && !char.IsWhiteSpace(l))) throw new ArgumentException("Invalid name");

            var cs = "DataSource=" + DatabaseFileLocation.ConnectString;
            using (var con = new SQLiteConnection(cs))
            {
                con.Open();
                var stm = "insert into birthday(location, name, birthday, birthmonth, birthyear) values(@location, @name, @birthday, @birthmonth, @birthyear);";

                using (var cmd = new SQLiteCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = stm;
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@location", birthAddDto.Location);
                    cmd.Parameters.AddWithValue("@name", birthAddDto.Name);
                    cmd.Parameters.AddWithValue("@birthday", birthAddDto.Day);
                    cmd.Parameters.AddWithValue("@birthmonth", birthAddDto.Month);
                    cmd.Parameters.AddWithValue("@birthyear", birthAddDto.Year);
                    await cmd.ExecuteNonQueryAsync();
                }

                con.Close();
            }
        }
    }
}