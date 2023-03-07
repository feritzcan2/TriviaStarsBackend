using Amazon.Runtime.Internal.Util;
using Api.Service.Repository.DbEntities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;

namespace Api.Service.Repository;

public class QuestionsInReviewRepository : RepositoryBase<DbQuestionInReview>
{
    List<DbQuestionInReview> _cache;
    public QuestionsInReviewRepository(IMongoClient client, IOptions<MongoDbConfiguration> configuration) : base(
        client, configuration)
    {
    }

    public async Task RemoveAllEntries()
    {
        await Collection.DeleteManyAsync(x => true);
    }

    public new async Task InsertAsync(DbQuestionInReview obj)
    {
        if (_cache == null)
        {
            _cache = await GetAllEntries();
        }
        _cache.Add(obj);
        await Collection.InsertOneAsync(obj);
    }

    public new async Task InsertManyAsync(IEnumerable<DbQuestionInReview> obj)
    {
        if (_cache == null)
        {
            _cache = await GetAllEntries();
        }
        _cache.AddRange(obj);
        await Collection.InsertManyAsync(obj);
    }
    public async Task<List<DbQuestionInReview>> GetAllEntries()
    {
        return await Collection.Find(x => true).ToListAsync();
    }

    public async Task<List<DbQuestionInReview>> GetPaged()
    {
        return await Collection.Find(x => true).ToListAsync();
    }

    public async Task<List<DbQuestionInReview>> GetAllCachedEntries()
    {
        if(_cache == null)
        {
            _cache = await GetAllEntries();
        }
        return _cache;
    }
    public  async Task<IReadOnlyList<DbQuestionInReview>> QueryByPage(int page, int pageSize)
    {
        var countFacet = AggregateFacet.Create("count",
        PipelineDefinition<DbQuestionInReview, AggregateCountResult>.Create(new[]
        {
                PipelineStageDefinitionBuilder.Count<DbQuestionInReview>()
            }));

        var dataFacet = AggregateFacet.Create("data",
        PipelineDefinition<DbQuestionInReview, DbQuestionInReview>.Create(new[]
        {
            PipelineStageDefinitionBuilder.Sort(Builders<DbQuestionInReview>.Sort.Ascending(x => x.Id)),
            PipelineStageDefinitionBuilder.Skip<DbQuestionInReview>((page - 1) * pageSize),
            PipelineStageDefinitionBuilder.Limit<DbQuestionInReview>(pageSize),
            }));

        var filter = Builders<DbQuestionInReview>.Filter.Eq(x=>x.Status , ReviewStatus.Waiting);
        var aggregation = await Collection.Aggregate()
            .Match(filter)
            .Facet(countFacet, dataFacet)
            .ToListAsync();

        var count = aggregation.First()
            .Facets.First(x => x.Name == "count")
            .Output<AggregateCountResult>()
            ?.FirstOrDefault()
            ?.Count ?? 0;

        var totalPages = (int)count / pageSize;

        var data = aggregation.First()
            .Facets.First(x => x.Name == "data")
            .Output<DbQuestionInReview>();

        return data;
    }
}