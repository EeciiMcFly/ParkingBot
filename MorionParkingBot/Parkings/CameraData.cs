namespace MorionParkingBot.Parkings;

public class CameraData
{
	public long Id { get; set; }

	public Guid CameraId { get; set; }

	public long ServerId { get; set; }

	public ServerData Server { get; set; }

	public long ParkingId { get; set; }

	public ParkingData Parking { get; set; }
}