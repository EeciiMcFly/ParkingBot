using System.Text.Json.Serialization;

namespace MorionParkingBot.Parkings.ToServerServices;

public class PointsDTO
{
	[JsonPropertyName("point_lt")]
	public Point PointLT { get; set; }

	[JsonPropertyName("point_rt")]
	public Point PointRT { get; set; }

	[JsonPropertyName("point_rb")]
	public Point PointRB { get; set; }

	[JsonPropertyName("point_lb")]
	public Point PointLB { get; set; }
}