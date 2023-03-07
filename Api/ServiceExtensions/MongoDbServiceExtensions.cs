using Api.Service.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace Api.ServiceExtensions;

public static class MongoDbServiceExtensions
{
    public static void AddMongoDb(this WebApplicationBuilder context, IServiceCollection services)
    {
        var mongoDbSection = context.Configuration.GetSection("MongoDbConfiguration");
        services.Configure<MongoDbConfiguration>(setting =>
        {
            mongoDbSection.Bind(setting);
        });
        var connectionString = context.Configuration.GetValue<string>("MongoDbConfiguration:ConnectionString");
        services.AddSingleton<IMongoClient>(new MongoClient(connectionString));
        services.AddSingleton<QuestionsInReviewRepository>();
        services.AddSingleton<DbQuestionsRepository>();


    }

}