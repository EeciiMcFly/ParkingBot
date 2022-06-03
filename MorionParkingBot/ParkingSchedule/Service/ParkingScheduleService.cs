namespace MorionParkingBot.ParkingSchedule;

public class ParkingScheduleService : IParkingScheduleService
{
	private readonly IParkingScheduleRepository _parkingScheduleRepository;

	public ParkingScheduleService(IParkingScheduleRepository parkingScheduleRepository)
	{
		_parkingScheduleRepository = parkingScheduleRepository;
	}

	public async Task CreateParkingScheduleAsync(long userId, long parkingId, DateTime time)
	{
		var parkingScheduleData = new ParkingScheduleData
		{
			Time = time,
			UserId = userId,
			ParkingId = parkingId
		};

		await _parkingScheduleRepository.SaveAsync(parkingScheduleData);
	}

	public async Task<List<ParkingScheduleData>> GetParkingScheduleByTimeAsync(DateTime start, DateTime end)
	{
		var searchParams = new ParkingScheduleSearchParams
		{
			StartTime = start,
			EndTime = end
		};

		return await _parkingScheduleRepository.GetAsync(searchParams);
	}
}