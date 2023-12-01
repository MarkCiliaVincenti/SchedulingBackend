namespace EventScheduler.Postgres
{
    public class PostgresConfiguration
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Port { get; set; }
        public string User {  get; set; }
        public string Password { get; set; }

        public PostgresConfiguration()
        {
            Server = Environment.GetEnvironmentVariable("server");
            Port = Environment.GetEnvironmentVariable("port");
            Database = Environment.GetEnvironmentVariable("database");
            User = Environment.GetEnvironmentVariable("user");
            Password = Environment.GetEnvironmentVariable("password");
        }
    }
}
