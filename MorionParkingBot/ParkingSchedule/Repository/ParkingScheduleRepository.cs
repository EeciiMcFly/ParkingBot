using Microsoft.EntityFrameworkCore;

namespace MorionParkingBot.ParkingSchedule;

public class ParkingScheduleRepository : IParkingScheduleRepository
{
	private readonly ParkingScheduleDbContext _dbContext;

	public ParkingScheduleRepository(ParkingScheduleDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task SaveAsync(ParkingScheduleData parkingSchedule)
	{
		_dbContext.ParkingSchedules.Add(parkingSchedule);

		await _dbContext.SaveChangesAsync();
	}

	public async Task DeleteAsync(ParkingScheduleData parkingSchedule)
	{
		_dbContext.ParkingSchedules.Remove(parkingSchedule);

		await _dbContext.SaveChangesAsync();
	}

	public async Task<List<ParkingScheduleData>> GetAsync(ParkingScheduleSearchParams searchParams)
	{
		var query = _dbContext.ParkingSchedules
			.Include(x => x.Parking)
			.Include(x => x.User)
			.AsQueryable();

		var isUserIdFilterExist = searchParams.UserId.HasValue;
		var isParkingIdFilterExist = searchParams.ParkingId.HasValue;
		var isStartTimeFilterExist = searchParams.StartTime.HasValue;
		var isEndTimeFilterExist = searchParams.EndTime.HasValue;

		if (isUserIdFilterExist)
			query = query.Where(x => x.UserId == searchParams.UserId.Value);

		if (isParkingIdFilterExist)
			query = query.Where(x => x.ParkingId == searchParams.ParkingId.Value);

		if (isStartTimeFilterExist)
			query = query.Where(x => x.Time > searchParams.StartTime);

		if (isEndTimeFilterExist)
			query = query.Where(x => x.Time < searchParams.EndTime);

		return await query.ToListAsync();
	}
}