using Microsoft.EntityFrameworkCore;

namespace MorionParkingBot.Parkings;

public class ParkingDbContext : DbContext
{
	public ParkingDbContext()
	{
	}

	public ParkingDbContext(DbContextOptions<ParkingDbContext> options) : base(options)
	{
		Database.Migrate();
	}

	public DbSet<ParkingData> Parkings { get; set; }

	public DbSet<CameraData> Cameras { get; set; }

	public DbSet<ServerData> Servers { get; set; }

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