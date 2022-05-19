using System.Text.Json.Serialization;
using SixLabors.ImageSharp;

namespace MorionParkingBot.Parkings.ToServerServices;

public class Point
{
	[JsonPropertyName("x")]
	public float X { get; set; }

	[JsonPropertyName("y")]
	public float Y { get; set; }

	public PointF ToImagePointF(int width, int height)
	{
		var pointF = new PointF(X*width, Y*height);

		return pointF;
	}
}