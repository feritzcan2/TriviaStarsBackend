using Amazon.Runtime.Internal.Util;
using Api.Service.Repository.DbEntities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;

namespace Api.Service.Repository;

public class DbQuestionsRepository : RepositoryBase<DbQuestion>
{
    List<DbQuestion> _cache;
    public DbQuestionsRepository(IMongoClient client, IOptions<MongoDbConfiguration> configuration) : base(
        client, configuration)
    {
    }

 

}