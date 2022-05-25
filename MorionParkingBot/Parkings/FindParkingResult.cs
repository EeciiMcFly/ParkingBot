using SixLabors.ImageSharp;

namespace MorionParkingBot.Parkings;

public class FindParkingResult
{
	public string ParkingName { get; set; }

	public bool IsFreeParkingFind { get; set; }

	public List<Image> Images { get; set; }
}