using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using GBVSFrameBot.Models;
using Microsoft.EntityFrameworkCore;
using GBVSFrameBot.Services;

namespace GBVSFrameBot.Modules
{
    [Name("FrameDataModule")]
    [Summary("A module that gives frame data on various fighting game.")]
    public class FrameDataModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commands;
        private readonly DatabaseService _database;

        public FrameDataModule(IServiceProvider services)
        {
            // Dependency Injection
            _commands = services.GetRequiredService<CommandService>();
            _database = services.GetRequiredService<DatabaseService>();
        }

        [Command("granblue")]
        [Alias("gb")]
        [Summary("Displays frame data for any granblue versus character.")]
        public async Task GranblueAsync(string characterName, string input)
        {
            GranblueDbContext gbCtx = new GranblueDbContext();
            EmbedBuilder embed = new EmbedBuilder();

            // Query for a list of inputs with a lambda expression based on the character name and input string.
            Expression<Func<GranblueData, bool>> predicate = g => g.Character.ToLower() == characterName.ToLower() && g.Input.ToLower() == input.ToLower();
            List<GranblueData> gbData = (List<GranblueData>)_database.getQueryResult<GranblueData>(predicate, gbCtx);

            // If the query returns null (there is no frame data), or does not return 1 value, query again for similair results.
            if (gbData.Count() != 1 || gbData == null)
            {
                // Query using a "LIKE" operator with comparison to the input.
                predicate = g => g.Character.ToLower() == characterName.ToLower() && EF.Functions.Like(g.Input.ToLower(), $"%{input.ToLower()}%");
                gbData = (List<GranblueData>)_database.getQueryResult<GranblueData>(predicate, gbCtx);

                // If the count is still 0, we found nothing. Construct an apology embed.            
                if (gbData.Count() == 0 || gbData == null)
                {
                    embed = new EmbedBuilder()
                    {
                        Color = new Color(255, 0, 0),
                        Description = $"{Context.User.Mention} Query had no results."
                    };
                }
                // If query returns some values, construct embed builder to reflect reccomended inputs.
                else
                {
                    string reccomendedInputs = "";
                    
                    // Append the input string for each of the queried moves.
                    foreach (GranblueData g in gbData)
                    {
                        reccomendedInputs += $"{g.Input} \n";
                    }

                    embed = new EmbedBuilder()
                    {
                        Color = new Color(255, 255, 0),
                        Description = $"{Context.User.Mention} I couldn't find '{input}' for '{characterName}'. \n I have some similar looking ones though:"
                    };
                    embed.AddField(x => { x.Name = "Reccomended Inputs"; x.Value = reccomendedInputs; x.IsInline = false; });
                }
            }
            // If the query returns with one value that is not null, construct a successful embed.
            else
            {
                embed = new EmbedBuilder()
                {
                    Color = new Color(0, 204, 0),
                    ImageUrl = gbData.First().ImageUrl,
                    ThumbnailUrl = gbData.First().ThumbnailUrl,
                    Author = new EmbedAuthorBuilder().WithName(gbData.First().Character.ToUpper()).WithUrl(gbData.First().DustloopCharacterUrl),
                    Description = $"**{gbData.First().Name}** \n\u200B"
                };

                embed.AddField(x => { x.Name = "Input"; x.Value = gbData.First().Input; x.IsInline = true; });
                embed.AddField(x => { x.Name = "Damage"; x.Value = gbData.First().Damage; x.IsInline = true; });
                embed.AddField(x => { x.Name = "Guard"; x.Value = gbData.First().Guard; x.IsInline = true; });
                embed.AddField(x => { x.Name = "Startup"; x.Value = gbData.First().StartUp; x.IsInline = true; });
                embed.AddField(x => { x.Name = "Active"; x.Value = gbData.First().Active; x.IsInline = true; });
                embed.AddField(x => { x.Name = "Recovery"; x.Value = gbData.First().Recovery; x.IsInline = true; });
                embed.AddField(x => { x.Name = "On Block"; x.Value = gbData.First().OnBlock; x.IsInline = true; });
                embed.AddField(x => { x.Name = "On Hit"; x.Value = gbData.First().OnHit; x.IsInline = true; });
                embed.AddField(x => { x.Name = "Level"; x.Value = gbData.First().OnHit; x.IsInline = true; });
            }

            await ReplyAsync("", false, embed.Build());
        }
    }
}