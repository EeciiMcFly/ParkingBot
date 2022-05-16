using Microsoft.EntityFrameworkCore;

namespace MorionParkingBot.PromoCodes;

public class PromoCodeDbContext : DbContext
{
	public PromoCodeDbContext()
	{
		
	}

	public PromoCodeDbContext(DbContextOptions<PromoCodeDbContext> options) : base(options)
	{
		Database.Migrate();
	}
	
	public DbSet<PromoCodeData> PromoCodes { get; set; }
	
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