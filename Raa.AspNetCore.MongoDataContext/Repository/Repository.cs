using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Raa.AspNetCore.MongoDataContext.Repository
{
    public class Repository<TEntity> : Repository<TEntity, MongoDataContext>
        where TEntity : IEntity<ObjectId>
    {
        public Repository(MongoDataContext context) : base(context)
        {

        }
    }
    public class Repository<TEntity, TContext>
        where TContext : MongoDataContext
        where TEntity : IEntity<ObjectId>
    {
        public readonly TContext _context;

        public readonly IMongoCollection<TEntity> _collection;
        public virtual IQueryable<TEntity> List => _collection.AsQueryable();


        public Repository(TContext context)
        {
            _context = context;
            _collection = _context.Database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async Task<TEntity> InsertAsync(TEntity e)
        {
            if (e == null) { throw new ArgumentNullException(nameof(e)); }

            e.Id = ObjectId.GenerateNewId();
            
            await _collection.InsertOneAsync(e);

            return e;
        }

        public async Task UpdateAsync(TEntity e)
        {
            await _collection.ReplaceOneAsync<TEntity>(i => i.Id == e.Id, e);
        }


        public async Task DeleteAsync(TEntity e)
        {
            await _collection.DeleteOneAsync<TEntity>(i => i.Id == e.Id);
        }

        public async Task DeleteAsync(ObjectId eId)
        {
            await _collection.DeleteOneAsync<TEntity>(i => i.Id == eId);
        }

        public async Task DeleteAsync(IEnumerable<TEntity> eList)
        {
            foreach(TEntity e in eList)
            {
                await _collection.DeleteOneAsync<TEntity>(i => i.Id == e.Id);
            }
        }

        public Task<TEntity> FindByIdAsync(ObjectId id)
        {
            return _collection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterFunc)
        {
            return _collection.Find(filterFunc).FirstOrDefaultAsync();
        }
        public Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filterFunc)
        {
            return _collection.Find(filterFunc).ToListAsync();
        }

        public Task<long> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return _collection.CountAsync(filter);
        }




    }
}
