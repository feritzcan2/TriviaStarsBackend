namespace Api.Service.Repository
{
    public class MongoDbConfiguration
    {
        public string DatabaseName { get; set; }

        public string DatabaseUsername { get; set; }

        public string DatabasePassword { get; set; }

        public string ConnectionString { get; set; }
    }
}
