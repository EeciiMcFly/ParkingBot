namespace MorionParkingBot.Parkings;

public interface IParkingsRepository
{
	Task<List<ParkingData>> GetAllParkingsAsync();

	Task<ParkingData> GetParkingById(long parkingId);

	Task<List<CameraData>> GetCameraRangeById(List<long> cameraIds);
}