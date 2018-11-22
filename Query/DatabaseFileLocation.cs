using System;

namespace birthdayservice.Query
{
    public static class DatabaseFileLocation
    {
        public static string ConnectString;
        public static void Setup(string location)
        {
            ConnectString = location + ";"; //"DataSource =C:/sqlite/notes.db;";
        }

        public static void Setup()
        {
            //Setup("/c/temp/notes.db");
            var location = EnvironmentHelper.DatabaseLocation;
            Console.Error.WriteLine("Database location: " + location);
            Setup(location);
        }
    }

    public static class EnvironmentHelper
    {
        private const string DebugLocation = "C:\\sqlite\\birtday.db";
        public const string EnvironmentArguments = "DATABASEPATH";
        public static string DatabaseLocation
        {
            get
            {
                var environmentVariables = Environment.GetEnvironmentVariables();
                if (!environmentVariables.Contains(EnvironmentArguments))
                {
                    return DebugLocation;
                }

                var argumentsHolder = environmentVariables[EnvironmentArguments] as string;
                var location = argumentsHolder?.Trim()?.Split();
                if (location?.Length == 0 || location == null)
                {
                    return DebugLocation;
                }

                return location[0];
            }
        }
    }
}