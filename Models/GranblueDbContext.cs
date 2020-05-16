using Microsoft.EntityFrameworkCore;

namespace GBVSFrameBot.Models
{
    public class GranblueDbContext : DbContext
    {
        public DbSet<GranblueData> GranblueData {get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./framedata.db");
        }
    }
}