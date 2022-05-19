using System.Text.Json.Serialization;

namespace MorionParkingBot.Parkings.ToServerServices;

public class CountingChannelDTO
{
	[JsonPropertyName("id")]
	public Guid Id { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("zones")]
	public List<ParkingZoneDTO> Zones { get; set; }
}