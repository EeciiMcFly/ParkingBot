using Microsoft.EntityFrameworkCore;

namespace MorionParkingBot.Database;

public class UserDbContext : DbContext
{
	public UserDbContext()
	{
	}

	public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
	{
		Database.Migrate();
	}

	public DbSet<UserData> Users { get; set; }
	
	public DbSet<LicenseInfo> LicenseInfos { get; set; }

	// Метод переопределяется для создания миграций
	// При работе сервиса контекст определяется в autofac и IsConfigured будет true
	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		if (!options.IsConfigured)
		{
			options.UseNpgsql("Host=localhost; Port=5432;Database=morionbot;Username=postgres;Password=masterkey");
		}
	}
}