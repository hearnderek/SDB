namespace SDB
{
    /// <summary>
    /// This poor man's singleton config is terrible, but it is here as a quick placeholder
    /// </summary>
    public static class Config
    {
        public static string dbFileName = "db.s";

        public static string fileLocation = System.IO.Path.Combine(System.Environment.CurrentDirectory, dbFileName);

        public static Database db = new Database();
    }
}
