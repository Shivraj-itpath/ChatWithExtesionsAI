using System.ClientModel;
using Microsoft.Extensions.AI;
using OpenAI;

namespace AIChatClient_BE.Services
{
    public static class AIServiceConfiguration
    {
        public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
        {
            builder.AddAIServices();
            return builder;
        }

        private static void AddAIServices(this IHostApplicationBuilder builder)
        {
            var loggerFactory = builder.Services.BuildServiceProvider().GetService<ILoggerFactory>();
            string? apiKey = builder.Configuration["AI:OpenAI:ApiKey"];
            string? openAiEndpoint = builder.Configuration["AI:OpenAI:Endpoint"];

            if (!string.IsNullOrWhiteSpace(openAiEndpoint))
            {
                builder.Services.AddChatClient(
                    new OpenAIClient(new ApiKeyCredential(apiKey),
                    new OpenAIClientOptions { Endpoint = new Uri(openAiEndpoint) }
                    )
                    .AsChatClient("gpt-4o-mini"))
                    .UseFunctionInvocation()
                    .UseOpenTelemetry(configure: t => t.EnableSensitiveData = true)
                    .UseLogging(loggerFactory)
                    .Build();
            }
        }
    }
}
