using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PTManagementSystem.Domain.Entities;

namespace PTManagementSystem.Infrastructure.Data
{
    public class UserDbContext
    {
        private readonly IMongoDatabase _database;

        public UserDbContext(IOptions<DatabaseSettings> databaseSettings)
        {
            var client = new MongoClient(databaseSettings.Value.ConnectionString);
            _database = client.GetDatabase(databaseSettings.Value.DatabaseName);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");

       
    }
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
    }
}
