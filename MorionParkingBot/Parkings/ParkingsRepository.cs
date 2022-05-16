using Microsoft.EntityFrameworkCore;

namespace MorionParkingBot.Parkings;

public class ParkingsRepository : IParkingsRepository
{
	private readonly ParkingDbContext _parkingDbContext;

	public ParkingsRepository(ParkingDbContext parkingDbContext)
	{
		_parkingDbContext = parkingDbContext;
	}

	public async Task<List<ParkingData>> GetAllParkingsAsync()
	{
		var parkings = await _parkingDbContext.Parkings.ToListAsync();

		return parkings;
	}

	public async Task<ParkingData> GetParkingByNameAsync(string parkingName)
	{
		var parking = await _parkingDbContext.Parkings.Include(x => x.Cameras)
			.FirstOrDefaultAsync(parling => parling.Name.Equals(parkingName));

		return parking;
	}
}