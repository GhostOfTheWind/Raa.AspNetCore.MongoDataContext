using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raa.AspNetCore.MongoDataContext
{
    public class MongoDataContext
    {
        private IMongoDatabase _database;

        public IMongoDatabase Database => _database;

        public MongoDataContext(IOptions<MongoDataContextOptions> options)
        {
            
            try
            {
                var mongoClient = new MongoClient(options.Value.ConnectionString);
                _database = mongoClient.GetDatabase(options.Value.DatabaseName);

            }
            catch (Exception ex)
            {
                throw new Exception("Can not access to db server. ", ex);
            }
        }
    }


    public class MongoDataContextOptions
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
