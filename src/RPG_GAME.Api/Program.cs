using RPG_GAME.Application;
using RPG_GAME.Core;
using RPG_GAME.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCore();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();
app.UseInfrastructure();

app.Run();

public partial class Program { }