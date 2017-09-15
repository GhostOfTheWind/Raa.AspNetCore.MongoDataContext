using MongoDB.Bson;
using Raa.AspNetCore.MongoDataContext.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDataContextSample.Entities
{
    public class Item : IEntity<ObjectId>
    {

        public ObjectId Id { get; set; }

        public string Name { get; set; }
        public string State { get; set; }
    }
}
