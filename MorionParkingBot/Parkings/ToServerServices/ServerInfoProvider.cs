using System.Text.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace MorionParkingBot.Parkings.ToServerServices;

public class ServerInfoProvider
{
	public async Task<CountingChannelDTO> GetInfoAboutParkingFromServerAsync(CameraData camera)
	{
		try
		{
			var server = camera.Server;
			var requestUrl = $"{server.ServerUrl}/api/parking?login={server.Login}&password=";
			var httpClient = new HttpClient();
			var result = await httpClient.GetAsync(requestUrl);
			var jsonString = await result.Content.ReadAsStringAsync();
			var сountingChannelsDTO = JsonSerializer.Deserialize<CountingChannelsDTO>(jsonString);
			var cameraFromServer = сountingChannelsDTO.CountingChannels.FirstOrDefault(x => x.Id == camera.CameraId);
			return cameraFromServer;
		}
		catch (Exception e)
		{
			var emptyResult = new CountingChannelDTO
			{
				Zones = new List<ParkingZoneDTO>
				{
					new()
					{
						IsFree = false
					}
				}
			};

			return emptyResult;
		}
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