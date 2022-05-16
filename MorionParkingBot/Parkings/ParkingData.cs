namespace MorionParkingBot.Parkings;

public class ParkingData
{
	public long Id { get; set; }

	public string Name { get; set; }

	public List<CameraData> Cameras { get; set; }
}