namespace birthdayservice.Utilities
{
    public static class MonthNames
    {
        private static string[] Months =
        {
            "Januar", "Februar", "Mars", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Desember"
        };
        public static string GetMonthName(this int number)
        {
            if(number < 1 || number > 12) return "Invalid month";
            return Months[number - 1];
        }
    }
}