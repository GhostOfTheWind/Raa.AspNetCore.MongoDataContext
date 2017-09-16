using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.MongoDb.Entities
{
    public class IdentityDbContext<TUser> : DbContext
        where TUser : IdentityUser
    {

        public IdentityDbContext() : base()
        {

        }


        

        public IMongoCollection<TUser> Users
        {
            get
            {
                return _database.GetCollection<TUser>("Users");
            }
        }

    }
}
