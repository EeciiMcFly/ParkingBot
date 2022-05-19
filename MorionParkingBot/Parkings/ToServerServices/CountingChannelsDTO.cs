using System.Text.Json.Serialization;

namespace MorionParkingBot.Parkings.ToServerServices;

public class CountingChannelsDTO
{
	[JsonPropertyName("objcounting_channels")]
	public List<CountingChannelDTO> CountingChannels { get; set; }
}