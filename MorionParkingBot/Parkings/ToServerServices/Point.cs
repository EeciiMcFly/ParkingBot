using SixLabors.ImageSharp;

namespace MorionParkingBot.Parkings.ToServerServices;

public class Point
{
	public float X { get; set; }

	public float Y { get; set; }

	public PointF ToImagePointF(int width, int height)
	{
		var pointF = new PointF(X*width, Y*height);

		return pointF;
	}
}