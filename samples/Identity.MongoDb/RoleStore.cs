using Identity.MongoDb.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Driver;
using System.Threading;

namespace Identity.MongoDb
{
    public class RoleStore<TRole> : IRoleStore<TRole>//, IQueryableRoleStore<TRole>
        where TRole : IdentityRole
    {
        private IMongoCollection<TRole> _roles;

        public RoleStore():this(new DbContext())
        {

        }
        public RoleStore(DbContext dbContext)
        {
            _roles = dbContext._database.GetCollection<TRole>("Roles");
        }

       
        public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            await _roles.InsertOneAsync(role);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {

            await _roles.DeleteOneAsync(r => r.Id == role.Id);
            return IdentityResult.Success; 
        }

        public void Dispose()
        {
            // ??
        }

        public async Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {

            return await _roles.Find(r => r.Id == roleId).FirstOrDefaultAsync(); ;
        }

        public async Task<TRole> FindByNameAsync(string roleName, CancellationToken cancellationToken)
        {
            
            return await _roles.Find(r => r.Name == roleName).FirstOrDefaultAsync(); 
        }

        public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            await _roles.ReplaceOneAsync(r => r.Id == role.Id, role);
            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<TRole> Roles => _roles.AsQueryable();
    }
}
