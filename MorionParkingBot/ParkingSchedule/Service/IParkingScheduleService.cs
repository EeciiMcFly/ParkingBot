namespace MorionParkingBot.ParkingSchedule;

public interface IParkingScheduleService
{
	Task CreateParkingScheduleAsync(long userId, long parkingId, DateTime time);
	Task<List<ParkingScheduleData>> GetParkingScheduleByTimeAsync(DateTime start, DateTime end);
}