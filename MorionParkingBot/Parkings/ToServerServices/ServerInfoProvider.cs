using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace MorionParkingBot.Parkings.ToServerServices;

public class ServerInfoProvider
{
	public async Task<ServerParkingInfo> GetInfoAboutParkingFromServerAsync(CameraData camera)
	{
		var server = camera.Server;

		var requestUrl = $"{server.ServerUrl}/api/parking/{camera.CameraId}";
		var requestResult = false;

		var pointLT = new Point
		{
			X = (float) 0.2755,
			Y = (float) 0.4788
		};
		var pointRT = new Point
		{
			X = (float) 0.5232,
			Y = (float) 0.7183
		};
		var pointRB = new Point
		{
			X = (float) 0.5108,
			Y = (float) 0.4741

		};
		var pointLB = new Point
		{
			X = (float) 0.2724,
			Y = (float) 0.2730
		};

		var zone = new Zone
		{
			PointLT = pointLT,
			PointRT = pointRT,
			PointRB = pointRB,
			PointLB = pointLB
		};

		var parkingInfo = new ParkingsInfo
		{
			Id = 1,
			Points = zone,
			IsFree = true
		};

		var serverParkingInfo = new ServerParkingInfo
		{
			Parkings = new List<ParkingsInfo> {parkingInfo}
		};

		return serverParkingInfo;
	}

	public async Task<Image> GetRealtimeFrameFromServer(CameraData camera)
	{
		var server = camera.Server;
		var requestUrl = $"{server.ServerUrl}/site?login={server.Login}&channelid={camera.CameraId}&resolutionx=1600&resolutiony=900";
		var httpClient = new HttpClient();
		var stream = await httpClient.GetStreamAsync(requestUrl);
		var config = Configuration.Default;
		config.MaxDegreeOfParallelism = 1;
		Image image;
		using (var ms = new MemoryStream())
		{
			await stream.CopyToAsync(ms);
			ms.Position = 0;
			var imageDecoder = new JpegDecoder();
			image = Image.Load(config, ms, imageDecoder);
		}

		return image;
	}
}