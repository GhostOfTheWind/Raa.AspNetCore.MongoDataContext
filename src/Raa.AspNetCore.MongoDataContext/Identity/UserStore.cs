using Microsoft.AspNetCore.Identity;
using Raa.AspNetCore.MongoDataContext.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Claims;
using Raa.AspNetCore.MongoDataContext.Repository;
using MongoDB.Bson;
using System.Linq;

namespace Raa.AspNetCore.MongoDataContext.Identity
{
    public class UserStore<TUser> : IUserStore<TUser>,
        IUserLoginStore<TUser>,
        IUserClaimStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserSecurityStampStore<TUser>,
        IUserTwoFactorStore<TUser>,
        IUserEmailStore<TUser>,
        IUserLockoutStore<TUser>,
        IUserPhoneNumberStore<TUser>

        where TUser : MongoIdentityUser
    {

        Repository<TUser> _userRepo;

        public UserStore(Repository<TUser> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            await _userRepo.InsertAsync(user);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            await _userRepo.DeleteAsync(user);
            return IdentityResult.Success;
        }


        public Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return _userRepo.FindByIdAsync(ObjectId.Parse(userId));
        }

        public Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = _userRepo.List.Where(u => 
                        u.Logins.Where(l => 
                            l.LoginProvider == loginProvider && 
                            l.ProviderKey == providerKey).Count() >= 0).FirstOrDefault();

            return Task.FromResult(user);
        }

        public Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return _userRepo.FindOneAsync(u => u.NormalizedEmail == normalizedEmail);
        }

        public Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return _userRepo.FindOneAsync(u => u.NormalizedUserName == normalizedUserName);
        }


        public Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            user.AddLogin(login);
            return Task.FromResult(0);
        }


        public Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            var claims = user.Claims.Select(clm => new Claim(clm.Type, clm.Value)).ToList();

            return Task.FromResult<IList<Claim>>(claims);
        }

        public Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)   { throw new ArgumentNullException(nameof(user)); }
            if (claims == null) { throw new ArgumentNullException(nameof(claims)); }

            foreach (var claim in claims)
            {
                user.AddClaim(claim);
            }

            return Task.CompletedTask;
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            return Task.FromResult<IList<UserLoginInfo>>(user.Logins);
        }

        public Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            return Task.FromResult(user.EmailConfirmed);
        }



        public void Dispose()
        {
            //
        }


        public Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            return Task.FromResult(user.AccessFailedCount);
        }


        public Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            var lockoutEndDate = user.LockoutEndDateUtc != null
                ? user.LockoutEndDateUtc 
                : default(DateTimeOffset?);

            return Task.FromResult(lockoutEndDate);
        }

        

        public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            return Task.FromResult(user.NormalizedEmail);
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            return Task.FromResult(user.SecurityStamp);
        }

        public Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            return Task.FromResult(user.UserName);
        }

        public Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (claim == null) { throw new ArgumentNullException(nameof(claim)); }

            var users = _userRepo.List.Where(u => 
                u.Claims.Where(c => c.Type == claim.Type && c.Value == claim.Value).Count() >= 0);

            return Task.FromResult<IList<TUser>>(users.ToList());
        }

        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            return Task.FromResult(user.PasswordHash != null);
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }


            return Task.FromResult(user.AccessFailedCount++);
        }

        public Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }
            if (claims == null) { throw new ArgumentNullException(nameof(claims)); }

            foreach(var claim in claims)
            {
                user.RemoveClaim(claim);
            }

            return Task.CompletedTask;
        }

        public Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }
            if (loginProvider == null) { throw new ArgumentNullException(nameof(loginProvider)); }
            if (loginProvider == null) { throw new ArgumentNullException(nameof(providerKey)); }

            var loginToRemove = user.Logins.FirstOrDefault(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey);

            user.RemoveLogin(loginToRemove);

            return Task.CompletedTask;
        }

        public Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }
            if (claim == null) { throw new ArgumentNullException(nameof(claim)); }

            user.RemoveClaim(claim);
            user.AddClaim(newClaim);

            return Task.CompletedTask;
        }

        public Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            user.AccessFailedCount = 0;

            return Task.CompletedTask;
        }

        public Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            user.Email = email;

            return Task.CompletedTask;
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            user.EmailConfirmed = confirmed;

            return Task.CompletedTask;
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            user.LockoutEnabled = enabled;

            return Task.CompletedTask;
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }


            user.LockoutEndDateUtc = lockoutEnd;

            return Task.CompletedTask;
        }

        public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }


            user.NormalizedEmail = normalizedEmail;

            return Task.CompletedTask;
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }


            user.NormalizedUserName = normalizedName;

            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }


            user.PasswordHash = passwordHash;

            return Task.CompletedTask;
        }

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }


            user.PhoneNumber = phoneNumber;

            return Task.CompletedTask;
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }


            user.PhoneNumberConfirmed = confirmed;

            return Task.CompletedTask;
        }

        public Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }


            user.SecurityStamp = stamp;

            return Task.CompletedTask;
        }

        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }


            user.TwoFactorEnabled = enabled;

            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }


            user.UserName = userName;

            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) { throw new ArgumentNullException(nameof(user)); }


            await _userRepo.UpdateAsync(user);

            return IdentityResult.Success;
        }
    }
}
