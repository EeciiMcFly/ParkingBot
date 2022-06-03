using Microsoft.EntityFrameworkCore;
using MorionParkingBot.Parkings;
using MorionParkingBot.Users;

namespace MorionParkingBot.ParkingSchedule;

[Index(nameof(Time))]
public class ParkingScheduleData
{
	public long Id { get; set; }
	public DateTime Time { get; set; }
	public long UserId { get; set; }
	public UserData User { get; set; }
	public long ParkingId { get; set; }
	public ParkingData Parking { get; set; }
}