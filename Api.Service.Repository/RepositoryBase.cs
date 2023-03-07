using Api.Service.Repository.DbEntities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Api.Service.Repository
{
    public abstract class RepositoryBase<TDocument> where TDocument : BaseEntity
    {
        protected readonly IMongoCollection<TDocument> Collection;

        protected RepositoryBase(IMongoClient client, IOptions<MongoDbConfiguration> configuration)
        {
            var mongoConfig = configuration.Value;
            var collectionName = BsonCollectionAttribute.GetCollectionName(typeof(TDocument));
            Collection = client.GetDatabase(mongoConfig.DatabaseName)
                .GetCollection<TDocument>(collectionName);
        }

        public async Task<TDocument> GetByIdAsync(string id)
        {
            return await Collection.Find(x => x.Id == id).SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(TDocument obj)
        {
            await Collection.ReplaceOneAsync(x => x.Id == obj.Id, obj);
        }
        public async Task InsertAsync(TDocument obj)
        {
            await Collection.InsertOneAsync(obj);
        }

        public async Task InsertManyAsync(IEnumerable<TDocument> obj)
        {
            await Collection.InsertManyAsync(obj);
        }
        public async Task RemoveAsync(string id)
        {
            await Collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task DeleteManyAsync(IEnumerable<TDocument> obj)
        {
            if (obj.Count() == 0) return;
            var ids = obj.Select(x => x.Id);
            await Collection.DeleteManyAsync(x => ids.Contains(x.Id));
        }
    }

}
