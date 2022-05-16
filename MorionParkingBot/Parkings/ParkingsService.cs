namespace MorionParkingBot.Parkings;

public class ParkingsService : IParkingsService
{
	private readonly IParkingsRepository _parkingsRepository;

	public ParkingsService(IParkingsRepository parkingsRepository)
	{
		_parkingsRepository = parkingsRepository;
	}

	public async Task<List<ParkingData>> GetParkingForIkmAsync()
	{
		var parkingsForIkm = await _parkingsRepository.GetAllParkingsAsync();

		return parkingsForIkm;
	}
}