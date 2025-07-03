using MongoDB.Bson;
using MongoDB.Driver;
using BatePapoServer.Hubs;


var builder = WebApplication.CreateBuilder(args);

//configuração MongoDB
var connectionString = builder.Configuration.GetConnectionString("MongoDb");
var client = new MongoClient(connectionString);
var database = client.GetDatabase("ChatHub");
builder.Services.AddSingleton(database);
try
{
    var collections = database.ListCollectionNames().ToList();
}
catch (Exception ex)
{
    Console.WriteLine($"Erro ao conectar ao MongoDB: {ex.ToString()}");
}
//Inserir documento inicial na coleção TestCollerction
var collection = database.GetCollection<BsonDocument>("TestCollection");
if(collection.CountDocuments(new BsonDocument()) == 0)
{
    collection.InsertOne(new BsonDocument("init",true));
}
//Testando conexão com o MongoDb e listando banco de dados
var testClient = new MongoClient("mongodb://localhost:27017");
var dbs = testClient.ListDatabaseNames().ToList();
Console.WriteLine(string.Join(",", dbs));
//Configura SignalR
builder.Services.AddSignalR();
//Add services to the container
builder.Services.AddControllers();
//Configuração permissões CORS mais ampla para o desenvolvimento
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(host => true);//permitir qualquer origem acessar
        });

});
builder.WebHost.UseUrls("https://localhost:5000");
var app = builder.Build();
//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors();
}
app.UseRouting();
app.UseAuthorization();
app.MapHub<ChatHub>("/chathub");
app.MapControllers();
app.MapGet("/", () => "API SignalR Chat Server rodondo");
app.Run();



