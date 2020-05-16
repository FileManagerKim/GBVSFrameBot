using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace GBVSFrameBot.Modules
{
    [Name("HelpModule")]
    [Summary("A module that helps users identify commands.")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commands;
        private readonly IConfiguration _config;

        public HelpModule(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _config = services.GetRequiredService<IConfiguration>();
        }

        [Command("commands")]
        [Summary("Lists all of the modules and their commands.")]
        public async Task HelpAsync()
        {
            // Construct an embed builder to help neatly format an embedded discord msg
            var builder = new EmbedBuilder()
            {
                Color = new Color(0,204,0),
                Description = "The following are all the available modules and their commands:"
            };

            // Iterate through each command in each module
            foreach(var module in _commands.Modules)
            {
                string description = null;

                foreach(var cmd in module.Commands)
                {
                    // Check if context is permitted to use/display the command
                    if((await cmd.CheckPreconditionsAsync(Context)).IsSuccess)
                    {
                        description += $"{_config["prefix"]}{cmd.Aliases.FirstOrDefault()}\n"; 
                    }
                }
                // Insert module name along with its commands
                if (!string.IsNullOrWhiteSpace(description))
                {
                    builder.AddField(x =>
                    {
                        x.Name = module.Name;
                        x.Value = description;
                        x.IsInline = false;
                    });
                }
            }

            await ReplyAsync("",false,builder.Build());
        }

        [Command("help")]
        [Summary("Shows info for the requested command.")]
        public async Task HelpAsync(string command)
        {
            var result = _commands.Search(Context, command);

            // Check for command existence
            if (!result.IsSuccess)
            {
                await ReplyAsync($"Sorry, I couldn't find: **{command}**.");
                return;
            }

            // Construct an embed builder to help neatly format an embedded discord msg
            var builder = new EmbedBuilder()
            {
                Color = new Color(0, 204, 0),
                Description = $"Here are the commands matching: **{command}**"
            };

            // Iterate through each command that matches
            foreach (var match in result.Commands)
            {
                var cmd = match.Command;

                // Add the command to the builder
                builder.AddField(x =>
                {
                    x.Name = string.Join(", ", cmd.Aliases);
                    x.Value = $"Parameters: {string.Join(", ", cmd.Parameters.Select(p => p.Name))}\n" + 
                              $"Summary: {cmd.Summary}";
                    x.IsInline = false;
                });
            }

            await ReplyAsync("", false, builder.Build());
        }
    }
}