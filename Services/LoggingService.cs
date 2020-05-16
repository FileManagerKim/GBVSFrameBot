using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace GBVSFrameBot.Services
{
    public class LoggingService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;

        public LoggingService(IServiceProvider services)
        {
            // Dependency Injection
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _commands = services.GetRequiredService<CommandService>();

            // Multi-cast Delegates
            _commands.Log += OnLogAsync;
            _discord.Log += OnLogAsync;
        }

        public async Task InitializeAsync()
        {
            // Generate start-up log message
            LogMessage msg = new LogMessage(
                LogSeverity.Info,
                "Service",
                "Logging service operational.",
                null);
            await OnLogAsync(msg);
        }

        private Task OnLogAsync(LogMessage msg)
        {
            // Generate generic log message
            Console.WriteLine($"[General/{msg.Severity}] {msg}");

            return Task.CompletedTask;
        }
    }
}