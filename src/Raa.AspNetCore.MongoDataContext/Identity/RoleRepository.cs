using Raa.AspNetCore.MongoDataContext.Identity.Entities;
using Raa.AspNetCore.MongoDataContext.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raa.AspNetCore.MongoDataContext.Identity
{
    public class RoleRepository<TRole> : Repository<TRole>
        where TRole : MongoIdentityRole
    {
        public RoleRepository(MongoDataContext context) : base(context, "Roles")
        {

        }
    }
}
