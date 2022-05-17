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

	public async Task<ParkingData> GetParkingById(long parkingId)
	{
		var parking = await _parkingDbContext.Parkings.Include(x => x.Cameras)
			.FirstOrDefaultAsync(parling => parling.Id.Equals(parkingId));

		return parking;
	}

	public async Task<List<CameraData>> GetCameraRangeById(List<long> cameraIds)
	{
		var cameras = await _parkingDbContext.Cameras.Include(x => x.Server)
			.Where(x => cameraIds.Contains(x.Id)).ToListAsync();

		return cameras;
	}
}