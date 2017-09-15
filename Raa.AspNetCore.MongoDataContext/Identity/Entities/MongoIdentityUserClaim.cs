using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Raa.AspNetCore.MongoDataContext.Identity.Entities
{
    public class MongoIdentityUserClaim
    {
        public MongoIdentityUserClaim() { }

        public MongoIdentityUserClaim(Claim claim)
        {
            Type = claim.Type;
            Value = claim.Value;
        }

        public string Type { get; set; }
        public string Value { get; set; }


        public Claim ToSecurityClaim()
        {
            return new Claim(Type, Value);
        }
    }
}
