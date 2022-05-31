using Microsoft.EntityFrameworkCore;

namespace MorionParkingBot.Metrics;

public class UsingDbContext : DbContext
{
	public UsingDbContext()
	{
	}

	public UsingDbContext(DbContextOptions<UsingDbContext> options) : base(options)
	{
		Database.Migrate();
	}

	public DbSet<UsingModel> Usings { get; set; }

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