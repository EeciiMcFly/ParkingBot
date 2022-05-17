using MorionParkingBot.Parkings.ToServerServices;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace MorionParkingBot.Parkings;

public class ParkingsService : IParkingsService
{
	private readonly IParkingsRepository _parkingsRepository;
	private readonly ServerInfoProvider _serverInfoProvider;

	public ParkingsService(IParkingsRepository parkingsRepository,
		ServerInfoProvider serverInfoProvider)
	{
		_parkingsRepository = parkingsRepository;
		_serverInfoProvider = serverInfoProvider;
	}

	public async Task<List<ParkingData>> GetParkingForIkmAsync()
	{
		var parkingsForIkm = await _parkingsRepository.GetAllParkingsAsync();

		return parkingsForIkm;
	}

	public async Task<FindParkingResult> FindParkingAsync(long parkingId)
	{
		var parkingData = await _parkingsRepository.GetParkingById(parkingId);
		var cameraIds = parkingData.Cameras.Select(x => x.Id).ToList();
		var cameras = await _parkingsRepository.GetCameraRangeById(cameraIds);
		var parkingInfoMap = new Dictionary<long, List<ParkingsInfo>>();
		foreach (var camera in cameras)
		{
			var parkingInfo = await _serverInfoProvider.GetInfoAboutParkingFromServerAsync(camera);
			var freeParkings = parkingInfo.Parkings.Where(x => x.IsFree).ToList();
			if (freeParkings.Count > 0)
				parkingInfoMap[camera.Id] = freeParkings;
		}

		if (!parkingInfoMap.Any())
		{
			var findParkingResult = new FindParkingResult
			{
				ParkingName = parkingData.Name,
				IsFreeParkingFind = false
			};

			return findParkingResult;
		}

		var parkingToProcess = parkingInfoMap.First();
		var cameraToProcess = cameras.FirstOrDefault(x => x.Id == parkingToProcess.Key);
		const double penWidthFactor = 0.002;
		var realtimeImage = await _serverInfoProvider.GetRealtimeFrameFromServer(cameraToProcess);
		foreach (var parkingsInfo in parkingToProcess.Value)
		{
			var pointLT = parkingsInfo.Points.PointLT.ToImagePointF(realtimeImage.Width, realtimeImage.Height);
			var pointRT = parkingsInfo.Points.PointRT.ToImagePointF(realtimeImage.Width, realtimeImage.Height);
			var pointRB = parkingsInfo.Points.PointRB.ToImagePointF(realtimeImage.Width, realtimeImage.Height);
			var pointLB = parkingsInfo.Points.PointLB.ToImagePointF(realtimeImage.Width, realtimeImage.Height);
			var penWidth = (int)(penWidthFactor * realtimeImage.Width);
			var pen = new Pen(Color.Red, penWidth);
			realtimeImage.Mutate(x => x.DrawLines(pen, pointLT, pointRT, pointRB, pointLB, pointLT));
		}

		var foundedParkingResult = new FindParkingResult
		{
			ParkingName = parkingData.Name,
			IsFreeParkingFind = true,
			Image = realtimeImage
		};

		return foundedParkingResult;
	}
}