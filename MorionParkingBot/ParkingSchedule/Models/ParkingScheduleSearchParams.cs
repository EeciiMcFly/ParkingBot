namespace MorionParkingBot.ParkingSchedule;

public class ParkingScheduleSearchParams
{
	public long? UserId { get; set; }
	public long? ParkingId { get; set; }

	public DateTime? StartTime { get; set; }

	public DateTime? EndTime { get; set; }
}