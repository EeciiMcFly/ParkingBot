using MorionParkingBot.Users;

namespace MorionParkingBot.Metrics;

public class UsingService : IUsingService
{
	private readonly IUsingRepository _usingRepository;

	public UsingService(IUsingRepository usingRepository)
	{
		_usingRepository = usingRepository;
	}

	public async Task CreateUsingAsync(UserData user)
	{
		var usingModel = new UsingModel
		{
			Timestamp = DateTime.UtcNow,
			UserId = user.Id
		};

		await _usingRepository.SaveUsingAsync(usingModel);
	}
}