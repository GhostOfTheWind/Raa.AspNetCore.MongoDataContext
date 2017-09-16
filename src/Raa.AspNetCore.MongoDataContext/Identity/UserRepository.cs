using Raa.AspNetCore.MongoDataContext.Identity.Entities;
using Raa.AspNetCore.MongoDataContext.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raa.AspNetCore.MongoDataContext.Identity
{
    public class UserRepository<TUser> : Repository<TUser>
        where TUser : MongoIdentityUser
    {
        public UserRepository(MongoDataContext context) : base(context, "Users")
        {

        }
    }
}
