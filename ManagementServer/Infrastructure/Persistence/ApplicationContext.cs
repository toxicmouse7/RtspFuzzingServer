using ManagementServer.Configuration.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace ManagementServer.Infrastructure.Persistence;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public DbSet<Models.RtpFuzzingPreset> RtpFuzzingPresets { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=rtsp.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RtpFuzzingPresetConfiguration());
    }
}