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
            get { return DebugLocation; } //TODO: Add environment variable when in docker.
        }
    }
}