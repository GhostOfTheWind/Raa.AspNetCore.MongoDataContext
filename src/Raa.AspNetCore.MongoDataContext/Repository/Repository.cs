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

        public Repository(MongoDataContext context, string collectionName) : base(context, collectionName)
        {
        }
    }
    public class Repository<TEntity, TContext>
        where TContext : MongoDataContext
        where TEntity : IEntity<ObjectId>
    {
        private string _collectionName;
        public string CollectionName => _collectionName; 


        private TContext _context;

        private IMongoCollection<TEntity> _collection;
        public virtual IQueryable<TEntity> List => _collection.AsQueryable();

        public Repository(TContext context, string collectionName)
        {
            _collectionName = collectionName != null? collectionName : typeof(TEntity).Name + "s";
            _context = context;

            _collection = _context.Database.GetCollection<TEntity>(_collectionName);
        }

        public Repository(TContext context) : this(context, null)
        {
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

        public Task<TEntity> FindByIdAsync(string id)
        {
            ObjectId parsedId;
            ObjectId.TryParse(id, out parsedId);

            return FindByIdAsync(parsedId);
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
