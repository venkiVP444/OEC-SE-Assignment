using System.Text.Json;
using Microsoft.AspNetCore.OData;
using RL.Data;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddSqlite<RLContext>("Data Source=Database.db");
builder.Services.AddControllers()
    .AddOData(options => options.Select().Filter().Expand().OrderBy())
    .AddJsonOptions(options => options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<EnableQueryFiler>();
});

var corsPolicy = "allowLocal";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:3000") // your frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.WebHost.UseUrls("http://localhost:10010");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RL v1");
    c.RoutePrefix = string.Empty;
});


app.UseCors(corsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
