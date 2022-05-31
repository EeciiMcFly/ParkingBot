namespace MorionParkingBot.Metrics;

public class UsingRepository : IUsingRepository
{
	private UsingDbContext _dbContext;

	public UsingRepository(UsingDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task SaveUsingAsync(UsingModel usingModel)
	{
		_dbContext.Usings.Add(usingModel);

		await _dbContext.SaveChangesAsync();
	}
}