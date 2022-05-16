namespace MorionParkingBot.Parkings;

public class ServerData
{
	public long Id { get; set; }

	public string ServerUrl { get; set; }

	public string Login { get; set; }

	public string Password { get; set; }

	public List<CameraData> Cameras { get; set; }
}