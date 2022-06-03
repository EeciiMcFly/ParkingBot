using Microsoft.EntityFrameworkCore;
using MorionParkingBot.Parkings;
using MorionParkingBot.Users;

namespace MorionParkingBot.ParkingSchedule;

public class ParkingScheduleDbContext : DbContext
{
	public ParkingScheduleDbContext()
	{
	}

	public ParkingScheduleDbContext(DbContextOptions<ParkingScheduleDbContext> options) : base(options)
	{
		Database.Migrate();
	}

	public DbSet<ParkingData> Parkings { get; set; }
	public DbSet<UserData> Users { get; set; }
	public DbSet<ParkingScheduleData> ParkingSchedules { get; set; }

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