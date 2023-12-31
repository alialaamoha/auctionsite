using System.Net;
using MongoDB.Driver;
using MongoDB.Entities;
using Polly;
using Polly.Extensions.Http;
using SearchService;
using SearchService.Services;
using ZstdSharp.Unsafe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(GetPolicy());

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseAuthorization();

app.MapControllers();

await DB.InitAsync("SearchDb", MongoClientSettings.FromConnectionString(builder.Configuration.GetConnectionString("MongoDbConnection")));

app.Lifetime.ApplicationStarted.Register(async () => {
    try{

        await DbInitializer.InitDb(app);

    } catch(Exception e){

        Console.WriteLine(e);
    }

});

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy() 
       => HttpPolicyExtensions.HandleTransientHttpError()
         .OrResult( msg => msg.StatusCode == HttpStatusCode.NotFound)
         .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));
