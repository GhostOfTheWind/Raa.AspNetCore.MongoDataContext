using MongoDB.Bson;
using Raa.AspNetCore.MongoDataContext.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace basicApiSample.Entities
{
    public class Item : IEntity
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
    }
}
