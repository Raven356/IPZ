using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.DAL.Context
{
    public class BlogMongoDBContext
    {
        private readonly IMongoDatabase _database;
        public BlogMongoDBContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<TEntity> GetCollection<TEntity>(string collectionName)
        {
            return _database.GetCollection<TEntity>(collectionName);
        }
    }
}
