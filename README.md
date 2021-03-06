# GBVSFrameBot
A Granblue Versus frame data discord bot built with the Discord.Net API. Designed to be quick and modular and can fundamentally work with any Dustloop frame data.  

# Quick Setup
1. To set up the bot to run on a local (or virtual) machine, first create your own local .NetCore project and clone the repository into the newly created project's main directory, replacing any duplicate folders and files. Try debugging to see if you will receive a token error.

2. Now to resolve the token error. Add a <common> folder to your <netcoreapp> folder (<netcoreapp> can be generated by running the debugger), and inside create a JSON config file labeled "config.txt". You will need to list your Discord token and prefix parameters here.

3. Double check the previous two steps, and then compile. Other necessary components will be generated on compile time, including the database and the entity migration data. 
> **NOTE** that you may need to restore the project to import the dependencies.  

# Common Questions
**Q: Why is there no compiled version available?**  
**A:** This version is uncompiled and unavailable on the discord bot repository because it was designed for dustloop moderators and devs to take and alter for any of their site's supported games. The bot however does not contain any secured information relative to the site, which is why I decided to make the source code public.

**Q: My database isn't working, its not getting a database exception but there are no frame data entries!**  
**A:** GBVSFrameBot uses a SQLite database. The database will not be populated with entries by default but will be generated empty on compile. Scrape your own data, or visit https://www.dustloop.com/forums/ and contact a Dustloop administrator for a database/excel file that can be imported.

**Q: I created a database for my game, but I'm getting an invalid entry exception. What did I do wrong?**  
**A:** GBVSFrameBot is designed to work for games that use the same metrics and recording format that is used by Dustloop, and therefore is only compatible with games under their umbrella. However the code can be easily adjusted to fit any framework, simply alter the FrameDataModule to suit your game's needs.  

# Special Thanks
Special thanks to the discord.net api channel and dustloop moderators for helping with performance optimization and database data.
