using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.MongoDb.Entities
{
    

    public class DbContext
    {
        public readonly IMongoDatabase _database;

        public DbContext()
        {
            try
            {
                /*MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(ConnectionString));
                */
                var mongoClient = new MongoClient("mongodb://raa:raa@ds133044.mlab.com:33044/asptest");
                _database = mongoClient.GetDatabase("asptest");
            }
            catch (Exception ex)
            {
                throw new Exception("Can not access to db server.", ex);
            }
        }
    }

    


}
