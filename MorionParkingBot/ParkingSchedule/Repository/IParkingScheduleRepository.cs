namespace MorionParkingBot.ParkingSchedule;

public interface IParkingScheduleRepository
{
	Task SaveAsync(ParkingScheduleData parkingSchedule);
	Task DeleteAsync(ParkingScheduleData parkingSchedule);
	Task<List<ParkingScheduleData>> GetAsync(ParkingScheduleSearchParams searchParams);
}