using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Linq;
using System.Security.Claims;

namespace Identity.MongoDb.Entities
{

    public class IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public TKey Id { get; set; }

        public string UserName { get; set; }

        [BsonIgnoreIfNull]
        public virtual string PasswordHash { get; set; }

        [BsonIgnoreIfNull]
        public List<UserLoginInfo> Logins { get; set; }

        [BsonIgnoreIfNull]
        public List<string> Roles { get; set; }

        [BsonIgnoreIfNull]
        public List<IdentityUserClaim> Claims = new List<IdentityUserClaim>(); 


        public virtual string Email { get; set; }
        public virtual bool EmailConfirmed { get; set; }

        public virtual string PhoneNumber { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }

        public virtual bool LockoutEnabled { get; set; }
        public virtual DateTime? LockoutEndDateUtc { get; set; }
        public virtual bool TwoFactorEnabled { get; set; }
        public virtual int AccessFailedCount { get; set; }
        public virtual string SecurityStamp { get; set; }


        public virtual void AddRole(string role)
        {
            Roles.Add(role);
        }
        public virtual void RemoveRole(string role)
        {
            Roles.Remove(role);
        }

        public virtual void AddLogin(UserLoginInfo login)
        {
            Logins.Add(login);
        }
        public virtual void RemoveLogin(UserLoginInfo login)
        {
            var loginsToRemove = Logins
                .Where(l => l.LoginProvider == login.LoginProvider)
                .Where(l => l.ProviderKey == login.ProviderKey);

            Logins = Logins.Except(loginsToRemove).ToList();
        }

        public virtual void AddClaim(Claim claim)
        {
            Claims.Add(new IdentityUserClaim(claim));
        }
        public virtual void RemoveClaim(Claim claim)
        {
            var claimsToRemove = Claims
                .Where(c => c.Type == claim.Type)
                .Where(c => c.Value == claim.Value);

            Claims = Claims.Except(claimsToRemove).ToList();
        }


    }

    public class IdentityUser : IdentityUser<string>
    {
        public IdentityUser()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        public IdentityUser(string userName) : this()
        {
            UserName = userName;
        }
    }
}
