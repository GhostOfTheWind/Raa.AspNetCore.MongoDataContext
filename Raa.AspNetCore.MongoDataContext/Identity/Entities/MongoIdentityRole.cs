using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Raa.AspNetCore.MongoDataContext.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raa.AspNetCore.MongoDataContext.Identity.Entities
{
    public class MongoIdentityRole : IEntity<ObjectId>
    {
        public MongoIdentityRole()
        {
            Id = ObjectId.GenerateNewId();
        }

        public MongoIdentityRole(string roleName) : this()
        {
            Name = roleName;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Name { get; set; }

        [BsonIgnoreIfNull]
        public string NormalizedName { get; set; }
    }
}
