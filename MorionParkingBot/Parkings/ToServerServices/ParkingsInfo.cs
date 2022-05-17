namespace MorionParkingBot.Parkings.ToServerServices;

public class ParkingsInfo
{
	public long Id { get; set; }

	public Zone Points { get; set; }

	public bool IsFree { get; set; }
}