using System;
using System.IO;
using System.Threading.Tasks;
using GBVSFrameBot.Services;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace GBVSFrameBot
{
    class Program
    {
        private IConfiguration _config;

        static void Main(string[] args)
         => new Program().MainAsync().GetAwaiter().GetResult();

         public async Task MainAsync()
         {
            // Initialize configuration service
            _config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory, "common"))
            .AddJsonFile("config.json")
            .Build();

             using(var services = ConfigureServices())
             {
                // Startup client
                var client = services.GetRequiredService<DiscordSocketClient>();
                await client.LoginAsync(TokenType.Bot, _config["token"]);
                await client.StartAsync();

                // Initialize services
                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();
                await services.GetRequiredService<LoggingService>().InitializeAsync();

                await Task.Delay(-1);
             }
         }

         private ServiceProvider ConfigureServices()
         {
             return new ServiceCollection()
             .AddSingleton(_config)
             .AddSingleton<DiscordSocketClient>()
             .AddSingleton<CommandService>()
             .AddSingleton<CommandHandlingService>()
             .AddSingleton<LoggingService>()
             .AddSingleton<DatabaseService>()
             .BuildServiceProvider();
         }
    }
}
