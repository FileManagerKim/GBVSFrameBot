using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace GBVSFrameBot.Services
{
    public class CommandHandlingService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _services;


        public CommandHandlingService(IServiceProvider services)
        {
            // Dependency Injection
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _commands = services.GetRequiredService<CommandService>();
            _config = services.GetRequiredService<IConfiguration>();
            _services = services;

            // Multi-cast delegates
            _discord.MessageReceived += MessageReceivedAsync;
            _commands.CommandExecuted += CommandExecutedAsync;
        }
        public async Task InitializeAsync()
        {
            // Register modules that are public and inherit ModuleBase<T>
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ignore recursive bot message calls
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            // Check for message prefix
            var argpos = 0;
            if(!message.HasStringPrefix(_config["prefix"], ref argpos)) return;

            // Execute command if matched
            var context = new SocketCommandContext(_discord, message);
            await _commands.ExecuteAsync(context, argpos, _services);
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // Handle unspecified command
            if (!command.IsSpecified)
            {
                await context.Channel.SendMessageAsync($"Command is not specified.");
                return;
            }
            // Handle successful command
            if (result.IsSuccess)
                return;

            // Handle failed command
            await context.Channel.SendMessageAsync($"Error: {result}");
        }
    }
}