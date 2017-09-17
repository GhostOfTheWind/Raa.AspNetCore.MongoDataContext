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
    public class RepositoryBase<TEntity, TContext, TEntityKey>
        where TContext : MongoDataContext
        where TEntity : IEntity<TEntityKey>
    {
        private string _collectionName;
        public string CollectionName => _collectionName;

        private TContext _context;

        private IMongoCollection<TEntity> _collection;
        public IMongoCollection<TEntity> Collection => _collection;

        public IQueryable<TEntity> List => _collection.AsQueryable();

        public RepositoryBase(TContext context) : this(context, null)
        {

        }

        public RepositoryBase(TContext context, string collectionName)
        {
            _collectionName = collectionName != null ? collectionName : typeof(TEntity).Name + "s";
            _context = context;

            _collection = _context.Database.GetCollection<TEntity>(_collectionName);
        }
    }

    public class Repository<TEntity, TContext, TEntityKey> : RepositoryBase<TEntity, TContext, TEntityKey>
        where TContext : MongoDataContext
        where TEntity : IEntity<TEntityKey>

    {

        public Repository(TContext context) : this(context, null)
        {
            
        }

        public Repository(TContext context, string collectionName) : base(context, collectionName)
        {
        }

        public virtual async Task<TEntity> InsertAsync(TEntity e)
        {
            if (e == null) { throw new ArgumentNullException(nameof(e)); }


            await Collection.InsertOneAsync(e);

            return e;
        }

        public virtual Task<TEntity> FindByIdAsync(TEntityKey id)
        {
            return Collection.Find(u => u.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public virtual async Task UpdateAsync(TEntity e)
        {
            await Collection.ReplaceOneAsync<TEntity>(i => i.Id.Equals(e.Id), e);
        }

        public virtual async Task DeleteAsync(TEntity e)
        {
            await Collection.DeleteOneAsync<TEntity>(i => i.Id.Equals(e.Id));
        }

        public virtual async Task DeleteAsync(TEntityKey eId)
        {
            await Collection.DeleteOneAsync<TEntity>(i => i.Id.Equals(eId));
        }

        public virtual Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterFunc)
        {
            return Collection.Find(filterFunc).FirstOrDefaultAsync();
        }
        public virtual Task<List<TEntity>> FindManyAsync(Expression<Func<TEntity, bool>> filterFunc)
        {
            return Collection.Find(filterFunc).ToListAsync();
        }

        public virtual Task<long> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return Collection.CountAsync(filter);
        }
    }

    
    public class Repository<TEntity, TContext> : Repository<TEntity, TContext, ObjectId>
        where TContext : MongoDataContext
        where TEntity : IEntity<ObjectId>
    {
        
        public Repository(TContext context, string collectionName) : base(context, collectionName)
        {
            
        }

        public Repository(TContext context) : this(context, null)
        {
        }

        public override async Task<TEntity> InsertAsync(TEntity e) 
        {
            if (e == null) { throw new ArgumentNullException(nameof(e)); }

            e.Id = ObjectId.GenerateNewId();
            
            await Collection.InsertOneAsync(e);

            return e;
        }


        public Task<TEntity> FindByIdAsync(string id)
        {
            ObjectId parsedId;
            ObjectId.TryParse(id, out parsedId);

            return FindByIdAsync(parsedId);
        }

    }

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
}
