using System.ClientModel;
using AIChatClient_BE.Services;
using AIChatClient_BE.Services.Interface;
using Microsoft.Extensions.AI;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);

//builder.AddApplicationServices();

builder.Services.AddSingleton(
    new OpenAIClient(new ApiKeyCredential(builder.Configuration["OpenAI:ApiKey"]), 
    new OpenAIClientOptions { Endpoint = new Uri(builder.Configuration["OpenAI:Endpoint"]) }
    ));

builder.Services.AddChatClient(services =>
    services.GetRequiredService<OpenAIClient>().AsChatClient("gpt-4o-mini"));

builder.Services.AddEmbeddingGenerator(services =>
    services.GetRequiredService<OpenAIClient>().AsEmbeddingGenerator("text-embedding-3-small"));

// Add services to the container
builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddControllers();
builder.Configuration.AddUserSecrets<Program>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
