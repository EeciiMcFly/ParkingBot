using System.Text.Json.Serialization;

namespace MorionParkingBot.Parkings.ToServerServices;

public class ParkingZoneDTO
{
	[JsonPropertyName("id")]
	public Guid Id { get; set; }

	[JsonPropertyName("points")]
	public PointsDTO Points { get; set; }

	[JsonPropertyName("is_free")]
	public bool IsFree { get; set; }
}