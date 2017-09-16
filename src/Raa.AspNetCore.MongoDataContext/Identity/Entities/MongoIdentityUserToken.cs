using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raa.AspNetCore.MongoDataContext.Identity.Entities
{
    public class MongoIdentityUserToken
    {
        public virtual string LoginProvider { get; set; }
        public virtual string Name { get; set; }
        public virtual string Value { get; set; }
    }
}
